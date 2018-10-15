using Branch.Packages.Models.External.Halo4;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public class Promotion
	{
		public string ID { get; set; }

		public int TypeId { get; set; }

		public int LifetimeXP { get; set; }

		public int LifetimeDoubleXP { get; set; }

		public int TotalDoubleXPGames { get; set; }

		public int TotalNormalXPGames { get; set; }
	}
}
