using Branch.Global.Libraries;
using Branch.Global.Services;

namespace Branch.Services.Token.Models
{
	public class Config
	{
		public string[] InternalKeys { get; set; }

		public ChromieTalkie.Config Puppeteer { get; set; }

		public ConfigAuthProviders AuthProviders { get; set; }

		public string RedisConnectionString { get; set; }

		public static Config CreateDefault()
		{
			return new Config
			{
				InternalKeys = new string[] {"test"},
				Puppeteer = null,
				RedisConnectionString = "redis://127.0.0.1:6379?db=0",
				AuthProviders = new ConfigAuthProviders
				{
					XboxLive = new ConfigAuthProvider
					{
						EmailAddress = LocalSecrets.GetConfigValue<string>("local", "token", "AuthProviders.XboxLive.EmailAddress"),
						Password = LocalSecrets.GetConfigValue<string>("local", "token", "AuthProviders.XboxLive.Password"),
					},
				},
			};
		}
	}

	public class ConfigAuthProviders
	{
		public ConfigAuthProvider XboxLive { get; set; }
	}

	public class ConfigAuthProvider
	{
		public string EmailAddress { get; set; }

		public string Password { get; set; }
	}
}
