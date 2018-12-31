using Apollo;
using Branch.Apps.ServiceXboxLive.App;
using Branch.Apps.ServiceXboxLive.Models;
using Branch.Apps.ServiceXboxLive.Server;
using Branch.Clients.Token;
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

namespace Branch.Apps.ServiceXboxLive
{
	public class Startup : ApolloStartup<Config>
	{
		public Startup(IHostingEnvironment environment)
			: base(environment, "service-xboxlive")
		{
			var tokenConfig = Configuration.Services["Token"];
			var tokenClient = new TokenClient(tokenConfig.Url, tokenConfig.Key);

			var app = new Application(tokenClient);
			var rpc = new RPC(app);

			RpcRegistration<RPC>(rpc);
		}

		public static async Task Main(string[] args) =>
			await new WebHostBuilder()
				.UseKestrel()
				.UseStartup<Startup>()
				.Build()
				.RunAsync();
	}
}
