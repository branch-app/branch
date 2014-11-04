using System.ComponentModel.DataAnnotations;

namespace Branch.Models.Sql
{
	public class ReachIdentity
		: Audit, IGameSpecificIdentity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string PlayerModelUrl { get; set; }

		public string ServiceTag { get; set; }

		[Required]
		public int CompetitiveKills { get; set; }

		[Required]
		public double KillDeathRatio { get; set; }

		public string Rank { get; set; }

		[Required]
		public int TotalGames { get; set; }


		public virtual GamerIdentity GamerIdentity { get; set; }
	}
}
