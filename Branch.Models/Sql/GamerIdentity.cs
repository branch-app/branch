using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Branch.Models.Sql
{
	public class GamerIdentity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string GamerId { get; set; }

		[Required]
		public string GamerIdSafe { get; set; }

		[Required]
		public IdentityType Type { get; set; }

		public virtual ICollection<Halo4Identity> Halo4Identities { get; set; }

		public virtual ICollection<ReachIdentity> ReachIdentities { get; set; }

		public virtual ICollection<BranchIdentity> BranchIdentities { get; set; } 

		public static string EscapeGamerId(string gamerId)
		{
			// To Lower
			return gamerId.ToLowerInvariant();
		}
	}

	public enum IdentityType
	{
		XblGamertag = 0x00,
		PsnTag = 0x20,
		PcOriginTag = 0x40,
		PcSteamTag = 0x50
	}
}
