using System;
using System.Linq;
using Branch.Core.Game.HaloReach.Helpers;
using Branch.Core.Game.HaloReach.Models._343.DataModels;

namespace Branch.App.Helpers.Razor.Reach
{
	public static class MetadataHelpers
	{
		public static Commendation GetCommendationById(int id)
		{
			return GlobalStorage.HReachManager.Metadata.Data.Commendations.FirstOrDefault(c => c.Id == id);
		}

		public static WeaponDetails GetWeaponDetailsById(int weaponId)
		{
			return (GlobalStorage.HReachManager.Metadata.Data.Weapons.FirstOrDefault(w => w.Id == weaponId) ?? new Weapon()).WeaponDetails;
		}

		public static MedalDetails GetMedalDetailsById(int medalId)
		{
			return (GlobalStorage.HReachManager.Metadata.Data.Medals.FirstOrDefault(m => m.Id == medalId) ?? new Medal()).MedalDetails;
		}

		public static EnemyDetails GetEnemyDetailsById(int enemyId)
		{
			return (GlobalStorage.HReachManager.Metadata.Data.Enemies.FirstOrDefault(e => e.Id == enemyId) ?? new Enemy()).EnemyDetails;
		}

		public static Tuple<string, int, string, int> GetNextAndCurrentCommendationLevel(int value,
			CommendationDetails commendationDetails)
		{
			if (value >= commendationDetails.Max)
				return new Tuple<string, int, string, int>("Max", commendationDetails.Max, null, -1);
			if (value >= commendationDetails.Onyx)
				return new Tuple<string, int, string, int>("Onyx", commendationDetails.Onyx, "Max", commendationDetails.Max);
			if (value >= commendationDetails.Gold)
				return new Tuple<string, int, string, int>("Gold", commendationDetails.Gold, "Onyx", commendationDetails.Onyx);
			if (value >= commendationDetails.Silver)
				return new Tuple<string, int, string, int>("Silver", commendationDetails.Silver, "Gold", commendationDetails.Gold);
			if (value >= commendationDetails.Bronze)
				return new Tuple<string, int, string, int>("Bronze", commendationDetails.Bronze, "Silver", commendationDetails.Silver);
			if (value >= commendationDetails.Iron)
				return new Tuple<string, int, string, int>("Iron", commendationDetails.Iron, "Bronze", commendationDetails.Bronze);
			
			return new Tuple<string, int, string, int>("Beginner", 0, "Iron", commendationDetails.Iron);
		}

		public static int GetCurrentCommendationTierIndex(string tier)
		{
			switch (tier)
			{
				case "Beginner":
					return 0;
				case "Iron":
					return 1;
				case "Bronze":
					return 2;
				case "Silver":
					return 3;
				case "Gold":
					return 4;
				case "Onyx":
					return 5;
				case "Max":
					return 6;

				default:
					return -1;
			}
		}

		public static string GetTeamColour(int teamId)
		{
			return AssetHelpers.TeamColourDictionary[teamId].Item1;
		}

		public static string GetTeamName(int teamId)
		{
			return AssetHelpers.TeamColourDictionary[teamId].Item2;
		}
	}
}