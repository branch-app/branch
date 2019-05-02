using Amazon;
using Amazon.S3;
using AutoMapper;
using Branch.Apps.ServiceHalo4.App;
using Branch.Apps.ServiceHalo4.Models;
using Branch.Apps.ServiceHalo4.Server;
using Branch.Apps.ServiceHalo4.Services;
using Branch.Clients.Token;
using Branch.Clients.Identity;
using Branch.Packages.Contracts.ServiceHalo4;
using Branch.Packages.Models.Halo4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Branch.Packages.Crpc.Registration;
using Branch.Packages.Crpc;
using Branch.Clients.Branch;
using Branch.Clients.S3;
using Microsoft.Extensions.Configuration;

namespace Branch.Apps.ServiceHalo4
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<ServiceRecordResponse, ResGetPlayerOverview>();
				cfg.CreateMap<RecentMatchesResponse, ResGetRecentMatches>();
				cfg.CreateMap<ServiceRecordResponse, ResGetServiceRecord>();
			});
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<BranchConfig>("Token", Configuration.GetSection("Services:Token"));
			services.Configure<BranchConfig>("Identity", Configuration.GetSection("Services:Identity"));
			services.Configure<S3Config>(Configuration.GetSection("S3"));

			services.AddSingleton<IdentityClient>();
			services.AddSingleton<TokenClient>();
			services.AddSingleton<S3Client>();
			services.AddSingleton<WaypointClient>();

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
				opts.RegisterMethod("get_service_record", "GetServiceRecord", "2018-09-12");
				opts.RegisterMethod("get_player_overview", "GetPlayerOverview", "2018-09-12");
				opts.RegisterMethod("get_recent_matches", "GetRecentMatches", "2018-09-12");
			});
		}

		public static async Task Main(string[] args) =>
			await CrpcHost.CreateCrpcHost<Startup>()
				.Build()
				.RunAsync();
	}
}
