using Branch.Core.Game.Halo4.Enums;

namespace Branch.Core.Game.Halo4.Models._343.DataModels
{
	public class CommendationModels
	{
		public class NextCommendationLevelData
		{
			public string NextLevelName { get; set; }

			public float ProgressToNextLevel { get; set; }

			public int NextLevelStartTicks { get; set; }
		}

		public class Commendation
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommendationCategory CategoryId { get; set; }

			public string CategoryName { get; set; }

			public int Ticks { get; set; }

			public int LevelId { get; set; }

			public string LevelName { get; set; }

			public string LevelStartTicks { get; set; }

			public NextCommendationLevelData NextLevel { get; set; }
		}

	}
}
