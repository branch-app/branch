using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Crypto;
using Branch.Packages.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Branch.Clients.S3
{
	public class S3Client
	{
		public AmazonS3Client Client { get; }

		public string BucketName { get; }

		public S3Client(IOptions<S3Config> options)
		{
			var opts = options.Value;
			var config = new AmazonS3Config
			{
				RegionEndpoint = RegionEndpoint.GetBySystemName(opts.Region),
			};

			Client = new AmazonS3Client(opts.AccessKeyId, opts.SecretAccessKey, config);
			BucketName = opts.BucketName;
		}

		public async Task CacheContent<T>(string key, T content, ICacheInfo cacheInfo)
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
					BucketName = BucketName,
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

				await Client.PutObjectAsync(putReq);
			}
		}
	}
}
