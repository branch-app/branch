using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Enemy
	{
		[JsonProperty("Key")]
		public int Id { get; set; }

		[JsonProperty("Value")]
		public EnemyDetails EnemyDetails { get; set; }
	}

	public class EnemyDetails
	{
		public string Description { get; set; }

		public int Id { get; set; }

		public string Name { get; set; }

		public string ImageName { get; set; }
	}
}
