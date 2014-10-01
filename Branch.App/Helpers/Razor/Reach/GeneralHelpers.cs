using System;
using System.Text.RegularExpressions;
using Branch.Core.Game.HaloReach.Helpers;

namespace Branch.App.Helpers.Razor.Reach
{
	public static class GeneralHelpers
	{
		public static int GetRankStartXp(string rankName)
		{
			return AssetHelpers.RankStartXpDictionary[rankName.ToLowerInvariant()];
		}

		public static Tuple<string, string> GetGameVictoryStatus(int standing)
		{
			switch (standing)
			{
				case 0:
					return new Tuple<string, string>("win", "Won");

				default:
					return new Tuple<string, string>("los", "Lost");
			}
		}

		public static string RemoveGuestIdentifier(string gamertag)
		{
			return Regex.Replace(gamertag, @"\((\d)\)", "", RegexOptions.None);
		}
	}
}