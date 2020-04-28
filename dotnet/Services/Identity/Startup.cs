using System.Threading.Tasks;
using Branch.Global.Attributes;
using Branch.Global.Contracts;
using Branch.Services.Identity.App;
using Branch.Services.Identity.Libraries;
using Branch.Services.Identity.Models;
using Branch.Services.Identity.Server;
using Branch.Services.Token;
using Crpc;
using Crpc.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceStack.Redis;

namespace Branch.Services.Identity
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
			services.Configure<Config>(Configuration);

			var redisConnectionString = Configuration.GetSection("RedisConnectionString").Get<string>();
			var tokenCfg = Configuration.GetSection("TokenConfig").Get<ServiceConfig>();

			services.AddSingleton<IRedisClientsManager>(new BasicRedisClientManager(redisConnectionString));
			services.AddSingleton(new TokenClient(tokenCfg.Url, tokenCfg.Key));
			services.AddSingleton<XblIdentityCache>();

			services.AddCrpc<Application, RpcServer>(opts =>
			{
				opts.InternalKeys = Configuration.GetSection("InternalKeys").Get<string[]>();
			});
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseCrpcHealthCheck();
			app.UseCrpc<RpcServer>("/1", (c, s) => {
				c.Authentication = AuthenticationType.AllowInternalAuthentication;

				c.Register<GetXblIdentityReq, GetXblIdentityRes>(
					"get_xbl_identity", "preview", s.GetXblIdentity, s.GetXblIdentitySchema
				);
			});
		}

		[ServiceEntry("service-identity")]
		public static async Task Entry(string[] args) =>
			await CrpcHost.CreateCrpcHost<Startup>(Config.CreateDefault())
				.UseConsoleLifetime()
				.Build()
				.RunAsync();
	}
}
