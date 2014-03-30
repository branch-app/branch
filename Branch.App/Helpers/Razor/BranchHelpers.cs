using System;
using System.Globalization;
using System.Text;
using Branch.Models.Services.Branch;

namespace Branch.App.Helpers.Razor
{
	public class BranchHelpers
	{
		public static string CheckGamertagPrivacy(string gamertag, Enums.GamerId gamerIdType = Enums.GamerId.X360XblGamertag)
		{
			//if (gamertag == "Doofette") return "Evie";
			return gamertag;
		}

		public static string GamertagToLeaf(string gamertag, bool alsoValidate = true)
		{
			//if (gamertag == "Doofette") gamertag = "Evie";
			return gamertag.Replace(" ", "_");
		}

		public static string NumberWithDelimiter(int number, string delimiter = ",")
		{
			return number >= 10000 ? number.ToString("n0") : number.ToString("d");
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
				sb.Append(years + " years, ");
				total++;
			}
			if (months > 0)
			{
				sb.Append(months + " months, ");
				total++;
			}
			if (weeks > 0)
			{
				sb.Append(weeks + " weeks, ");
				total++;
			}
			if (days <= 0) return sb.ToString();

			if (total > 0)
				sb.Append("and " + days + " days");
			else
				sb.Append(days + " days");

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
