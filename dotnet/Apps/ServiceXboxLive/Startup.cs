using Branch.Apps.ServiceXboxLive.App;
using Branch.Apps.ServiceXboxLive.Server;
using Branch.Clients.Branch;
using Branch.Clients.S3;
using Branch.Clients.Token;
using Branch.Packages.Contracts.ServiceXboxLive;
using Branch.Packages.Crpc;
using Branch.Packages.Crpc.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Branch.Apps.ServiceXboxLive
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
			services.Configure<S3Config>(Configuration.GetSection("S3"));

			services.AddSingleton<TokenClient>();
			services.AddSingleton<S3Client>();

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
			});
		}

		public static async Task Main(string[] args) =>
			await CrpcHost.CreateCrpcHost<Startup>()
				.Build()
				.RunAsync();
	}
}
