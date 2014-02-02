using System;
using System.Globalization;
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
