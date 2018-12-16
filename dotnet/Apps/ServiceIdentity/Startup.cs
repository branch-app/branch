using Apollo;
using Branch.Apps.ServiceIdentity.App;
using Branch.Apps.ServiceIdentity.Models;
using Branch.Apps.ServiceIdentity.Server;
using Branch.Apps.ServiceIdentity.Services;
using Branch.Clients.Token;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Models.Common.Config;
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

namespace Branch.Apps.ServiceIdentity
{
	public class Startup : ApolloStartup<Config>
	{
		public Startup(IHostingEnvironment environment)
			: base(environment, "service-identity")
		{
			var tokenConfig = Configuration.Services["Tokem"];
			var tokenClient = new TokenClient(tokenConfig.Url, tokenConfig.Key);
			var xblClient = new XboxLiveClient(tokenClient);
			var identityMapper = new IdentityMapper(xblClient);

			var app = new Application(tokenClient, identityMapper);
			var rpc = new RPC(app);

			RpcRegistration<RPC>(rpc);
			RegisterMethod<ReqGetXboxLiveIdentity, ResGetXboxLiveIdentity>("get_xboxlive_identity", "2018-08-19", rpc.GetXboxLiveIdentity, rpc.GetXboxLiveIdentitySchema);
		}

		public static async Task Main(string[] args) =>
			await new WebHostBuilder()
				.UseKestrel()
				.UseStartup<Startup>()
				.Build()
				.RunAsync();
	}
}
