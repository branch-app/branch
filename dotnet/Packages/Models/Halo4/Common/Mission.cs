using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.Common
{
	public class Mission
	{
		public int MapId { get; set; }

		[JsonProperty("mission")]
		public int MissionId { get; set; }

		public int Difficulty { get; set; }
	}
}
