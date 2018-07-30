using System;
using Apollo.Configuration.JsonObject;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Apollo.Configuration
{
	public static class ApolloConfiguration
	{
		public static IConfigurationRoot Add(IHostingEnvironment environment)
		{
			var configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(environment.ContentRootPath)
				.AddJsonFile("config.json", true)
				.AddJsonFile($"config.{environment.EnvironmentName}.json", true);

			// Read config environment variable, if it exists
			var configEnvVariable = Environment.GetEnvironmentVariable("CONFIG");
			if (configEnvVariable != null)
				configurationBuilder.AddJsonObject(JsonConvert.DeserializeObject(configEnvVariable));

			return configurationBuilder.Build();
		}
	}
}
