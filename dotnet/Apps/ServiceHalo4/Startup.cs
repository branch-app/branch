using Amazon;
using Amazon.S3;
using Apollo;
using AutoMapper;
using Branch.Apps.ServiceHalo4.App;
using Branch.Apps.ServiceHalo4.Models;
using Branch.Apps.ServiceHalo4.Server;
using Branch.Apps.ServiceHalo4.Services;
using Branch.Clients.Auth;
using Branch.Clients.Identity;
using Branch.Packages.Contracts.ServiceHalo4;
using Branch.Packages.Models.Common.Config;
using Branch.Packages.Models.Halo4;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Branch.Apps.ServiceHalo4
{
	public class Startup : ApolloStartup<Config>
	{
		public Startup(IHostingEnvironment environment)
			: base(environment, "service-halo4")
		{
			var authConfig = Configuration.Services["Auth"];
			var identityConfig = Configuration.Services["Identity"];
			var s3Config = Configuration.S3;

			var authClient = new AuthClient(authConfig.Url, authConfig.Key);
			var identityClient = new IdentityClient(identityConfig.Url, identityConfig.Key);
			var s3Client = new AmazonS3Client(s3Config.AccessKeyId, s3Config.SecretAccessKey, RegionEndpoint.GetBySystemName(s3Config.Region));
			var waypointClient = new WaypointClient(authClient, s3Client);

			var app = new Application(authClient, identityClient, waypointClient);
			var rpc = new RPC(app);

			// Setup automapper yaboi
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<ServiceRecordResponse, ResGetServiceRecord>();
			});

			RpcRegistration<RPC>(rpc);
			RegisterMethod<ReqGetServiceRecord, ResGetServiceRecord>("get_service_record", "2018-09-12", rpc.GetServiceRecord, rpc.GetServiceRecordSchema);
			RegisterMethod<ReqGetRecentMatches, ResGetRecentMatches>("get_recent_matches", "2018-09-12", rpc.GetRecentMatches, rpc.GetRecentMatchesSchema);
		}

		public static async Task Main(string[] args) =>
			await new WebHostBuilder()
				.UseKestrel()
				.UseStartup<Startup>()
				.Build()
				.RunAsync();
	}
}
