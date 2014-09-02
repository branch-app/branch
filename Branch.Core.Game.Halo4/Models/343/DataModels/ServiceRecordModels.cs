using Newtonsoft.Json;

namespace Branch.Core.Game.Halo4.Models._343.DataModels
{
	public class ServiceRecordModels
	{
		public class Promotion
		{
			public string Id { get; set; }

			public int TypeId { get; set; }

			[JsonProperty("LifetimeXP")]
			public int LifetimeXp { get; set; }

			[JsonProperty("LifetimeDoubleXP")]
			public int LifetimeDoubleXp { get; set; }

			public int TotalDoubleXpGames { get; set; }

			public int TotalNormalXpGames { get; set; }
		}

	}
}
