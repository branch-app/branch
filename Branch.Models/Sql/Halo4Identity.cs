using System.ComponentModel.DataAnnotations;

namespace Branch.Models.Sql
{
	public class Halo4Identity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string PlayerModelUrl { get; set; }

		[Required]
		public string ServiceTag { get; set; }

		[Required]
		public int TotalKills { get; set; }

		[Required]
		public double KillDeathRatio { get; set; }

		[Required]
		public int TopCsr { get; set; }

		[Required]
		public string FavouriteWeapon { get; set; }

		public virtual GamerIdentity GamerIdentity { get; set; }
	}
}
