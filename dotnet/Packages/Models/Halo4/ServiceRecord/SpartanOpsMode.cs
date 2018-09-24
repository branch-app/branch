using Branch.Packages.Models.Halo4.Common;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public class SpartanOpsMode : GameMode
	{
		public int? TotalSinglePlayerMissionsCompleted { get; set; }

		public int? TotalCoopMissionsCompleted { get; set; }

		public int? TotalMissionsPossible { get; set; }

		public int? TotalMedals { get; set; }

		public int? TotalGamesWon { get; set; }
	}
}
