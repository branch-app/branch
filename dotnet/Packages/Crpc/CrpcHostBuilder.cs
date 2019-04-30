using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Branch.Packages.Crpc
{
	public class CrpcHost
	{
		internal CrpcHost() { }

		public static IWebHostBuilder CreateCrpcHost<T>()
			where T : class
		{
			return new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config.AddCrpcConfig(hostingContext.HostingEnvironment);
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddConsole();
					logging.AddDebug();
					logging.AddEventSourceLogger();
				})
				.UseStartup<T>()
				.UseSentry();
		}
	}
}
