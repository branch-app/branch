using Newtonsoft.Json;

namespace Branch.Apps.ServiceHalo4.Models.Waypoint.Common
{
	public class Mission
	{
		public int MapId { get; set; }

		[JsonProperty("Mission")]
		public int MissionId { get; set; }

		public int Difficulty { get; set; }
	}
}
