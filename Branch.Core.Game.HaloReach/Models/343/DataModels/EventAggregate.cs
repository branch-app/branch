using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class EventAggregate
	{
		public int PenaltyPoints { get; set; }

		public int PlayerBetrayedAiCount { get; set; }

		public double PlayerKilledAiAverageDistanceInMeters { get; set; }

		public int PlayerKilledAiCount { get; set; }

		public double[] PlayerKilledAiDistancesInMeters { get; set; }

		public double PlayerKilledAiPerHour { get; set; }

		public int[] PlayerKilledAiTimeIndexes { get; set; }

		public double PlayerKilledByAiAverageDistanceInMeters { get; set; }

		public int PlayerKilledByAiCount { get; set; }

		public double[] PlayerKilledByAiDistancesInMeters { get; set; }

		public int[] PlayerKilledByAiTimeIndexes { get; set; }

		public int Points { get; set; }

		[JsonProperty("aiTypeClass")]
		public int AiTypeClass { get; set; }
	}
}
