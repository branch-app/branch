using Branch.Packages.Models.External.Halo4;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public class FavoriteWeapon
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string ImageUrl { get; set; }

		public int TotalKills { get; set; }
	}
}
