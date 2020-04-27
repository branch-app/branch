namespace Branch.Services.Token.Models
{
	public class Config
	{
		public string[] InternalKeys { get; set; } = new string[] { "test" };

		public string RemotePuppeteerEndpoint { get; set; }

		public ConfigProviders ConfigProviders { get; set; }
	}

	public class ConfigProviders
	{
		public ConfigProvider XboxLive { get; set; }
	}

	public class ConfigProvider
	{
		public string EmailAddress { get; set; }

		public string Password { get; set; }
	}
}
