using System.ComponentModel;

namespace Branch.Models.Services.Halo4._343.DataModels
{
	public class Enums
	{
		public enum GameMode
		{
			[Description("War Games")]
			WarGames = 3,

			[Description("Campaign")]
			Campaign = 4,

			[Description("Spartan Ops")]
			SpartanOps = 5,

			[Description("Custom Games")]
			Customs = 6
		}

		public enum ChallengeCategory
		{
			Campaign = 0,
			SpartanOps = 1,
			WarGames = 2,
			Waypoint = 3
		}

		public enum CommendationCategory
		{
			Weapons = 1,
			Enemies = 3,
			Vehicles = 4,
			Player = 5,
			GameTypes = 7
		}

		public enum Result
		{
			Lost = 0,
			Draw = 1,
			Won = 2
		}
	}
}
