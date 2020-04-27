using System.Threading.Tasks;
using Branch.Global.Attributes;
using Branch.Global.Services;
using Branch.Services.Token.App;
using Branch.Services.Token.Models;
using Crpc;
using Crpc.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Branch.Services.Token
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
			services.Configure<string>(Configuration.GetSection("RemotePuppeteerEndpoint"));

			services.AddSingleton<ChromieTalkie>();

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

				c.Register<GetTokenRequest, GetXblTokenResponse>("get_xbl_token", "preview", s.GetXblToken, s.GetXblTokenSchema);
			});
		}

		[ServiceEntry("service-token")]
		public static async Task Entry(string[] args) =>
			await CrpcHost.CreateCrpcHost<Startup>()
				.Build()
				.StartAsync();
	}
}
