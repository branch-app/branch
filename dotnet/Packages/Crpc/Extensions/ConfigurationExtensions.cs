using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Microsoft.Extensions.Configuration
{
	public static class ConfigurationExtensions
	{
		public static IConfigurationBuilder AddCrpcConfig(this IConfigurationBuilder builder, IHostingEnvironment environment)
		{
			builder
				.SetBasePath(environment.ContentRootPath)
				.AddJsonFile("config.json", true)
				.AddJsonFile($"config.{environment.EnvironmentName}.json", true);

			// Read config environment variable, if it exists
			var configEnvVariable = Environment.GetEnvironmentVariable("CONFIG");
			if (configEnvVariable != null)
				builder.AddJsonObject(JsonConvert.DeserializeObject(configEnvVariable));

			return builder;
		}
	}
}
