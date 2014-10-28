using System.Text.RegularExpressions;

namespace Branch.Models.Helpers
{
	public class ModernAgent
	{
		public ModernAgent(string userAgent)
		{
			UserAgent = userAgent;
		}

		public string UserAgent { get; private set; }

		public string Browser()
		{
			if (Regex.IsMatch(UserAgent, "konqueror", RegexOptions.IgnoreCase))
				return "Konqueror";
			if (Regex.IsMatch(UserAgent, "chrome", RegexOptions.IgnoreCase))
				return "Chrome";
			if (Regex.IsMatch(UserAgent, "safari", RegexOptions.IgnoreCase))
				return "Safari";
			if (Regex.IsMatch(UserAgent, @"trident\/(\d+).(\d+)", RegexOptions.IgnoreCase))
				return "Modern Internet Explorer";
			if (Regex.IsMatch(UserAgent, "msie", RegexOptions.IgnoreCase))
				return "Ancient Internet Explorer";
			if (Regex.IsMatch(UserAgent, "opera", RegexOptions.IgnoreCase))
				return "Opera";
			if (Regex.IsMatch(UserAgent, @"playstation (\d+)", RegexOptions.IgnoreCase))
				return "Playstation";
			if (Regex.IsMatch(UserAgent, "playstation portable", RegexOptions.IgnoreCase))
				return "PSP";
			if (Regex.IsMatch(UserAgent, "firefox", RegexOptions.IgnoreCase))
				return "Firefox";

			return "Unknown Browser";
		}

		public string BrowserEngine()
		{
			if (Regex.IsMatch(UserAgent, "webkit", RegexOptions.IgnoreCase))
				return "Webkit";
			if (Regex.IsMatch(UserAgent, "khtml", RegexOptions.IgnoreCase))
				return "KHTML";
			if (Regex.IsMatch(UserAgent, "konqueror", RegexOptions.IgnoreCase))
				return "Konqueror";
			if (Regex.IsMatch(UserAgent, "chrome", RegexOptions.IgnoreCase))
				return "Chrome";
			if (Regex.IsMatch(UserAgent, "presto", RegexOptions.IgnoreCase))
				return "Presto";
			if (Regex.IsMatch(UserAgent, @"trident\/(\d+).(\d+)", RegexOptions.IgnoreCase))
				return "Trident";
			if (Regex.IsMatch(UserAgent, "gecko", RegexOptions.IgnoreCase))
				return "Gecko";
			if (Regex.IsMatch(UserAgent, "msie", RegexOptions.IgnoreCase))
				return "MSIE";

			return "Unknown Browser Engine";
		}

		public string OperatingSystem()
		{
			if (Regex.IsMatch(UserAgent, @"windows nt 6\.4", RegexOptions.IgnoreCase))
				return "Windows 10 Technical Preview";
			if (Regex.IsMatch(UserAgent, @"windows nt 6\.3", RegexOptions.IgnoreCase))
				return "Windows 8.1";
			if (Regex.IsMatch(UserAgent, @"windows nt 6\.2", RegexOptions.IgnoreCase))
				return "Windows 8";
			if (Regex.IsMatch(UserAgent, @"windows nt 6\.1", RegexOptions.IgnoreCase))
				return "Windows 7";
			if (Regex.IsMatch(UserAgent, @"windows nt 6\.0", RegexOptions.IgnoreCase))
				return "Windows Vista";
			if (Regex.IsMatch(UserAgent, @"windows nt 5\.2", RegexOptions.IgnoreCase))
				return "Windows 2003";
			if (Regex.IsMatch(UserAgent, @"windows nt 5\.1", RegexOptions.IgnoreCase))
				return "Windows XP";
			if (Regex.IsMatch(UserAgent, @"windows nt 5\.0", RegexOptions.IgnoreCase))
				return "Windows 2000";
			if (Regex.IsMatch(UserAgent, @"linux", RegexOptions.IgnoreCase))
				return "Linux";
			if (Regex.IsMatch(UserAgent, @"wii", RegexOptions.IgnoreCase))
				return "Wii";
			if (Regex.IsMatch(UserAgent, @"playstation 3", RegexOptions.IgnoreCase))
				return "Playstation 3";
			if (Regex.IsMatch(UserAgent, @"playstation portable", RegexOptions.IgnoreCase))
				return "Playstation Portable";
			if (Regex.IsMatch(UserAgent, @"os x (\d+)[._](\d+)", RegexOptions.IgnoreCase))
				return "OS X";

			return "Unknown Operation System";
		}
	}
}
