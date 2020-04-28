namespace Branch.Services.Identity.Models
{
	public class Config
	{
		public string[] InternalKeys { get; set; }

		public string RedisConnectionString { get; set; }

		public static Config CreateDefault()
		{
			return new Config
			{
				InternalKeys = new string[] {"test"},
				RedisConnectionString = "redis://127.0.0.1:6379?db=1",
			};
		}
	}
}
