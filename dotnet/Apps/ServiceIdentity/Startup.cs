using Branch.Apps.ServiceIdentity.App;
using Branch.Apps.ServiceIdentity.Server;
using Branch.Apps.ServiceIdentity.Services;
using Branch.Clients.Branch;
using Branch.Clients.Token;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Crpc;
using Branch.Packages.Crpc.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Branch.Apps.ServiceIdentity
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<BranchConfig>("Token", Configuration.GetSection("Services:Token"));

			services.AddSingleton<TokenClient>();
			services.AddSingleton<XboxLiveClient>();
			services.AddSingleton<IdentityMapper>();

			services.AddSingleton<Application>();
			services.AddSingleton<IService, RPC>();

			services.AddCrpc(opts =>
			{
				opts.InternalKeys = Configuration.GetSection("InternalKeys").Get<string[]>();
			});
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseCrpc("/1", opts => {
				opts.Authentication = AuthenticationType.AllowInternalAuthentication;

				opts.RegisterServer<RPC>();
				opts.RegisterMethod("get_xboxlive_identity", "GetXboxLiveIdentity", "2018-08-19");
			});
		}

		public static async Task Main(string[] args) =>
			await CrpcHost.CreateCrpcHost<Startup>()
				.Build()
				.RunAsync();
	}
}
