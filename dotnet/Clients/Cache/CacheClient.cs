using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Crypto;
using Branch.Packages.Bae;
using Branch.Packages.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Branch.Clients.Cache
{
	public class CacheClient
	{
		private string bucket { get; set; }
		private AmazonS3Client client { get; set; }

		public CacheClient(string bucket, AmazonS3Client client) { }

		protected async Task<ICacheInfo> fetchCacheInfo(string key)
		{
			try
			{
				var resp = await client.GetObjectMetadataAsync(bucket, key);
				var contentCreation = DateTime.Parse(resp.Metadata["x-amz-meta-content-creation"]);
				var contentExpiration = DateTime.Parse(resp.Metadata["x-amz-meta-content-expiration"]);

				return new CacheInfo(contentCreation, contentExpiration);
			}
			catch (AmazonS3Exception ex) when (ex.ErrorCode == "NotFound")
			{
				return null;
			}
		}

		protected async Task<T> fetchContent<T>(string key)
		{
			try
			{
				var response = await client.GetObjectAsync(bucket, key);

				using (var ms = new MemoryStream())
				using (var sr = new StreamReader(ms))
				using (var jr = new JsonTextReader(sr))
				{
					await response.ResponseStream.CopyToAsync(ms);
					ms.Seek(0, SeekOrigin.Begin);

					var serializer = new JsonSerializer
					{
						ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
					};

					return serializer.Deserialize<T>(jr);
				}
			}
			catch (AmazonS3Exception ex)
			{
				if (ex.ErrorCode != "NotFound")
					throw;

				throw
					new BaeException(
						"cache_not_found",
						new Dictionary<string, object> { { "key", key } }
					);
			}
		}

		protected async Task cacheContent<T>(string key, T content, ICacheInfo cacheInfo)
		{
			using (var ms = new MemoryStream())
			using (var sw = new StreamWriter(ms))
			using (var jw = new JsonTextWriter(sw))
			{
				var serializer = new JsonSerializer();
				var now = DateTime.UtcNow;

				serializer.ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
				serializer.Serialize(jw, content);
				await sw.FlushAsync();

				var putReq = new PutObjectRequest
				{
					BucketName = bucket,
					Key = key,
					ContentType = "application/json",

					// We use the StreamWriter BaseStream, as apparently using a
					// MemoryStream breaks this part and only uploads an initial partial
					// segment of the data? Fuck knows
					InputStream = sw.BaseStream,
				};

				putReq.Metadata["content-expiration"] = cacheInfo.ExpiresAt?.ToISOString();
				putReq.Metadata["content-creation"] = cacheInfo.CachedAt.ToISOString();
				putReq.Metadata["content-hash"] = Sha256.HashContent(ms).ToHexString();

				await client.PutObjectAsync(putReq);
			}
		}
	}
}
