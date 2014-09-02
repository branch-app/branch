using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Medal
	{
		[JsonProperty("Key")]
		public int Id { get; set; }

		[JsonProperty("Value")]
		public MedalDetails MedalDetails { get; set; }
	}

	public class MedalDetails
	{
		public int Id { get; set; }

		public string Description { get; set; }

		public string ImageName { get; set; }

		public string Name { get; set; }

		public int Teir { get; set; }
	}
}
