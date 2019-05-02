using Amazon.SQS;
using AutoMapper;
using Branch.Apps.ServiceHalo2.App;
using Branch.Apps.ServiceHalo2.Database;
using Branch.Apps.ServiceHalo2.Models;
using Branch.Apps.ServiceHalo2.Server;
using Branch.Apps.ServiceHalo2.Services;
using Branch.Clients.Branch;
using Branch.Clients.Identity;
using Branch.Clients.Postgres;
using Branch.Clients.S3;
using Branch.Clients.Sqs;
using Branch.Packages.Contracts.ServiceHalo2;
using Branch.Packages.Crpc;
using Branch.Packages.Crpc.Registration;
using Branch.Packages.Extensions;
using Branch.Packages.Models.Common.Config;
using Branch.Packages.Models.Halo2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Branch.Apps.ServiceHalo2
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			// Setup automapper yaboi
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<ServiceRecord, ServiceRecordResponse>();
				cfg.CreateMap<ServiceRecordResponse, ResGetServiceRecord>();
			});
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<Config>(Configuration);
			services.Configure<BranchConfig>("Identity", Configuration.GetSection("Services:Identity"));
			services.Configure<SqsConfig>(Configuration.GetSection("Sqs"));
			services.Configure<S3Config>(Configuration.GetSection("S3"));
			services.Configure<PostgresConfig>(Configuration.GetSection("Postgres"));

			services.AddSingleton<IdentityClient>();

			services.AddSingleton<DatabaseClient>();
			services.AddSingleton<SqsClient>();
			services.AddSingleton<S3Client>();
			services.AddSingleton<BnetClient>();
			services.AddSingleton<IHostedService, QueueService>();

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
				opts.RegisterMethod("get_service_record", "GetServiceRecord", "2019-04-30");
			});
		}

		public static async Task Main(string[] args) =>
			await CrpcHost.CreateCrpcHost<Startup>()
				.Build()
				.RunAsync();
	}
}
