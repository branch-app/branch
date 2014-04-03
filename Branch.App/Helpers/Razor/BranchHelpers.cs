using System;
using System.Globalization;
using System.Text;
using Branch.Core.BranchStuff;
using Branch.Models.Services.Branch;

namespace Branch.App.Helpers.Razor
{
	public class BranchHelpers
	{
		public static string CheckGamerIdPrivacy(string gamerId, GamerId gamerIdType)
		{
			return GamerIdReplacementManager.GetReplacementGamerId(gamerId, GlobalStorage.AzureStorage, gamerIdType);
		}

		public static string GamertagToLeaf(string gamertag, bool alsoValidate = true)
		{
			return gamertag.Replace(" ", "_");
		}

		public static string NumberWithDelimiter(int number, string delimiter = ",")
		{
			return number >= 1000 ? number.ToString("n0") : number.ToString("d");
		}

		public static string MakeDateTimeFriendly(DateTime dateTime, string format = "G")
		{
			return dateTime.ToString(format);
		}

		public static string MakeTimeSpanFriendly(TimeSpan timeSpan)
		{
			var years = timeSpan.Days / 365; //no leap year accounting
			var months = (timeSpan.Days % 365) / 30; //naive guess at month size
			var weeks = ((timeSpan.Days % 365) % 30) / 7;
			var days = (((timeSpan.Days % 365) % 30) % 7);
			var total = 0;

			var sb = new StringBuilder();
			if (years > 0)
			{
				sb.Append(String.Format("{0} {1}, ", years, years == 1 ? "year" : "years"));
				total++;
			}
			if (months > 0)
			{
				sb.Append(String.Format("{0} {1}, ", months, months == 1 ? "month" : "months"));
				total++;
			}
			if (weeks > 0)
			{
				sb.Append(String.Format("{0} {1}, ", weeks, weeks == 1 ? "week" : "weeks"));
				total++;
			}
			if (days <= 0) return sb.ToString();

			sb.Append(total > 0
				? String.Format("and {0} {1}", days, days == 1 ? "day" : "days")
				: String.Format("{0} {1}", days, days == 1 ? "day" : "days"));

			return sb.ToString();
		}

		public static string MakeTimeSpanFriendlyPrecise(TimeSpan timeSpan)
		{
			var days = (((timeSpan.Days % 365) % 30) % 7);
			var hours = timeSpan.Hours;
			var minutes = timeSpan.Minutes;
			var seconds = timeSpan.Seconds;
			var total = 0;

			var sb = new StringBuilder();
			if (days > 0)
			{
				sb.Append(String.Format("{0} {1}, ", days, days == 1 ? "day" : "days"));
				total++;
			}
			if (hours > 0)
			{
				sb.Append(String.Format("{0} {1}, ", hours, hours == 1 ? "hour" : "hours"));
				total++;
			}
			if (minutes > 0)
			{
				sb.Append(String.Format("{0} {1}, ", minutes, minutes == 1 ? "minute" : "minutes"));
				total++;
			}
			if (seconds <= 0) return sb.ToString();

			sb.Append(total > 0
				? String.Format("and {0} {1}", seconds, seconds == 1 ? "second" : "seconds")
				: String.Format("{0} {1}", seconds, seconds == 1 ? "second" : "seconds"));

			return sb.ToString();
		}

		public static string CalculatePercentage(float a, float b, int roundTo = 2)
		{
			return Math.Round((a/b)*100, roundTo).ToString(CultureInfo.InvariantCulture);
		}

		public static string CompareEnum(object currentPage,
			object desiredPage, string output)
		{
			return currentPage.Equals(desiredPage) ? output : "";
		}
	}
}
