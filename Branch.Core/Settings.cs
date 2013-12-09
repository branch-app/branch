using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Branch.Core
{
	public static class Settings
	{
		// Wlid Settings
		public static string WlidAuthEmail = null;
		public static string WlidAuthPassword = null;

		// Twilio Settings
		public static string TwilioSid = null;
		public static string TwilioAuthToken = null;
		public static string TwilioFromNumber = null;
		public static string TwilioToNumber = null;

		public static bool NeedsSettings()
		{
			return (String.IsNullOrEmpty(WlidAuthEmail) ||
			        String.IsNullOrEmpty(WlidAuthPassword) ||
			        String.IsNullOrEmpty(TwilioSid) ||
			        String.IsNullOrEmpty(TwilioAuthToken) ||
			        String.IsNullOrEmpty(TwilioFromNumber) ||
			        String.IsNullOrEmpty(TwilioToNumber));
		}

		public static void LoadSettings(bool forceLoad = false)
		{
			if (!forceLoad && !NeedsSettings())
				return;

			var settings = JsonConvert.DeserializeObject<Dictionary<String, String>>(File.ReadAllText("settings.json"));

			WlidAuthEmail = settings["WlidAuthEmail"];
			WlidAuthPassword = settings["WlidAuthPassword"];

			TwilioSid = settings["TwilioSid"];
			TwilioAuthToken = settings["TwilioAuthToken"];
			TwilioFromNumber = settings["TwilioFromNumber"];
			TwilioToNumber = settings["TwilioToNumber"];
		}
	}
}