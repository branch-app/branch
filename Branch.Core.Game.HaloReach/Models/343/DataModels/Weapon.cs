using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Weapon
	{
		[JsonProperty("Key")]
		public int Id { get; set; }

		[JsonProperty("Value")]
		public WeaponDetails WeaponDetails { get; set; }
	}

	public class WeaponDetails
	{
		public int DamageClass { get; set; }

		public string Description { get; set; }

		public int Id { get; set; }

		public string ImageName { get; set; }

		public KeyValue<int>[] Metadata { get; set; }

		public string Name { get; set; }

		public string OfficialName { get; set; }
	}
}
