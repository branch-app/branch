using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Branch.Global.Libraries
{
	public static class LocalSecrets
	{
		private static string _configRepo { get; }
		private static string _infraRepo { get; }
		private static bool _hasRepo { get; } = true;

		static LocalSecrets()
		{
			_configRepo = Environment.GetEnvironmentVariable("BRANCH_CONFIG_REPO");
			_infraRepo = Environment.GetEnvironmentVariable("BRANCH_INFRA_REPO");

			if (String.IsNullOrWhiteSpace(_configRepo) || String.IsNullOrWhiteSpace(_configRepo))
				_hasRepo = false;
		}

		public static T GetConfigValue<T>(string environment, string service, string key)
		{
			if (!_hasRepo)
				return default(T);

			if (String.IsNullOrWhiteSpace(_configRepo))
				throw new Exception("Branch config repo environment variable not setup");

			if (!Directory.Exists(_configRepo))
				throw new DirectoryNotFoundException("Branch config repo doesn't exist");

			var path = Path.Combine(_configRepo, environment, $"service-{service}.json");

			if (!File.Exists(path))
				throw new FileNotFoundException($"Could not find config for config/{environment}/service-{service}");

			var obj = JObject.Parse(File.ReadAllText(path));
			var token = obj.SelectToken(key);

			return token.Value<T>();
		}
	}
}
