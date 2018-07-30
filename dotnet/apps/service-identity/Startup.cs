using System.Threading.Tasks;
using Branch.Apps.ServiceIdentity.Server;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using Apollo;
using Branch.Packages.Models.ServiceIdentity;

namespace Branch.Apps.ServiceIdentity
{
	public class Startup : ApolloStartup
	{
		public Startup(IHostingEnvironment environment)
			: base(environment, "service-identity") { }

		// This method gets called by the runtime. Use this method to add services to the container.
		public override void ConfigureServices(IServiceCollection services)
		{
			base.ConfigureServices(services);

			RegisterMethod<XboxLiveIdentityRequest, XboxLiveIdentityResponse>("submit_match", "2018-04-01", RPC.XboxLiveIdentity, RPC.XboxLiveIdentitySchema);
		}

		public static async Task Main(string[] args) =>
			await new WebHostBuilder()
				.UseStartup<Startup>()
				.Build()
				.RunAsync();
	}
}
