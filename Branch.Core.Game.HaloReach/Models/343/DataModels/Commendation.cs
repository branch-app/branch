using Branch.Core.Game.HaloReach.Enums;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Commendation
	{
		[JsonProperty("Key")]
		public int Id { get; set; }

		[JsonProperty("Value")]
		public CommendationDetails CommendationDetails { get; set; }
	}

	public class CommendationDetails
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int Max { get; set; }

		public int Onyx { get; set; }

		public int Gold { get; set; }

		public int Silver { get; set; }

		public int Bronze { get; set; }

		public int Iron { get; set; }

		public CommendationVariantClass Type { get; set; }
	}
}
