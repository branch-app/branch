using System.Collections.Generic;
using Branch.Global.Models.Domain;
using Branch.Global.Libraries;

namespace Branch.Services.Identity.Models
{
	public class Config
	{
		public string[] InternalKeys { get; set; }

		public string RedisConnectionString { get; set; }

		public ServiceConfig TokenConfig { get; set; }

		public static Config CreateDefault()
		{
			return new Config
			{
				InternalKeys = new string[] { "test" },
				RedisConnectionString = "redis://127.0.0.1:6379?db=1",
				TokenConfig = new ServiceConfig
				{
					Url = "https://service-token.branch.golf",
					Key = LocalSecrets.GetConfigValue<string>("prod", "token", "InternalKeys[0]"),
				}
			};
		}
	}
}
