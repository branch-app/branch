using Branch.Apps.ServiceHalo4.Models.Waypoint.Common;

namespace Branch.Apps.ServiceHalo4.Models.Waypoint.ServiceRecord
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
