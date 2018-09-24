using Branch.Packages.Models.Halo4.Common;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public class CampaignMode : GameMode
	{
		public DifficultyLevel[] DifficultyLevels { get; set; }

		public Mission[] SinglePlayerMissions { get; set; }

		public Mission[] CoopMissions { get; set; }

		public int TotalTerminalsVisited { get; set; }

		public long NarrativeFlags { get; set; }

		public object SinglePlayerDaso { get; set; }

		public object SinglePlayerDifficulty { get; set; }

		public object CoopDaso { get; set; }

		public object CoopDifficulty { get; set; }
	}
}
