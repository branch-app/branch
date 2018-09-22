using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Branch.Clients.Auth;
using Branch.Clients.Json;
using Branch.Clients.Json.Models;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceAuth;
using Branch.Packages.Crypto;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Exceptions;
using Branch.Packages.Extensions;
using Branch.Packages.Models.Halo4;
using Branch.Packages.Models.XboxLive;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Branch.Apps.ServiceHalo4.Services
{
	public class WaypointClient
	{
		private AuthClient authClient { get; }
		private AmazonS3Client s3Client { get; }
		private JsonClient presenceClient { get; }
		private JsonClient statsClient { get; }
		private JsonClient settingsClient { get; }
		private JsonClient optionsClient { get; }

		private const string presenceUrl = "https://presence.svc.halowaypoint.com/en-US/";
		private const string statsUrl = "https://stats.svc.halowaypoint.com/en-US/";
		private const string settingsUrl = "https://settings.svc.halowaypoint.com/";
		private const string optionsUrl = "https://settings.svc.halowaypoint.com/RegisterClientService.svc/register/webapp/AE5D20DCFA0347B1BCE0A5253D116752";
		private const string authHeader = "X-343-Authorization-Spartan";
		private const string storageBucket = "branch-app";

		public WaypointClient(AuthClient authClient, AmazonS3Client s3Client)
		{
			var jsonOptions = new Options(new Dictionary<string, string> {
				{ "accept", "application/json" },
			});

			this.authClient = authClient;
			this.s3Client = s3Client;
			this.presenceClient = new JsonClient(presenceUrl, jsonOptions);
			this.statsClient = new JsonClient(statsUrl, jsonOptions);
			this.settingsClient = new JsonClient(settingsUrl, jsonOptions);
			this.optionsClient = new JsonClient(optionsUrl, jsonOptions);
		}

		public async Task<(ServiceRecordResponse serviceRecord, ICacheInfo cacheInfo)> GetServiceRecord(string gamertag)
		{
			var path = $"players/{gamertag}/h4/servicerecord";
			var key = $"stats/halo-4/service-record/{gamertag}.json";
			var expire = TimeSpan.FromMinutes(10);

			return await makeWaypointRequest<ServiceRecordResponse>(path, key, expire);
		}

		private async Task<(T response, ICacheInfo cacheInfo)> makeWaypointRequest<T>(string path, string key, TimeSpan expire)
			where T : class
		{
			var now = DateTime.UtcNow;

			try
			{
				var metadata = await s3Client.GetObjectMetadataAsync(storageBucket, key);

				var cacheHash = metadata.Metadata["x-amz-meta-content-hash"];
				var cacheCreation = DateTime.Parse(metadata.Metadata["x-amz-meta-content-creation"]);
				var cacheExpiration = DateTime.Parse(metadata.Metadata["x-amz-meta-content-expiration"]);

				if (cacheExpiration > now)
				{
					return (
						await retrieveCacheContent<T>(key),
						new CacheInfo(cacheCreation, cacheExpiration)
					);
				}
			}
			catch (AmazonS3Exception ex)
			{
				// Ignore error is cache doesn't exist, this isn't exactly unexpected
				if (ex.ErrorCode != "NotFound")
					throw ex;
			}

			var auth = await getAuthHeaders();
			var cacheInfo = new CacheInfo(now, expire);
			var response = await statsClient.Do<T, Exception>("GET", path, null, new Options(auth));

			// Upload file to S3
			TaskExt.FireAndForget(() => cacheContent(key, response, cacheInfo));

			return (response, cacheInfo);
		}

		private async Task cacheContent<T>(string key, T content, ICacheInfo cacheInfo)
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
					BucketName = storageBucket,
					Key = key,
					ContentType = "application/json",

					// Use the StreamWriter BaseStream, as apparently using the MemStream
					// breaks it and only uploads a partial segment of the data??
					InputStream = sw.BaseStream,
				};

				putReq.Metadata["content-expiration"] = cacheInfo.ExpiresAt.ToISOString();
				putReq.Metadata["content-creation"] = cacheInfo.CachedAt.ToISOString();
				putReq.Metadata["content-hash"] = Sha256.HashContent(ms).ToHexString();

				await s3Client.PutObjectAsync(putReq);
			}
		}

		private async Task<T> retrieveCacheContent<T>(string key)
		{
			try
			{
				var response = await s3Client.GetObjectAsync(storageBucket, key);

				using (var ms = new MemoryStream())
				using (var sr = new StreamReader(ms))
				using (var jr = new JsonTextReader(sr))
				{
					await response.ResponseStream.CopyToAsync(ms);
					ms.Seek(0, SeekOrigin.Begin);

					var serializer = new JsonSerializer();
					serializer.ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
					return serializer.Deserialize<T>(jr);
				}
			}
			catch (AmazonS3Exception ex)
			{
				if (ex.ErrorCode == "NotFound")
					throw new BranchException("cache_no_longer_active", new Dictionary<string, object> { { "key", key } });

				throw ex;
			}
		}

		private async Task<Dictionary<string, string>> getAuthHeaders()
		{
			var resp = await authClient.GetHalo4Token();

			return new Dictionary<string, string> {{ "X-343-Authorization-Spartan", resp.SpartanToken }};
		}
	}
}
