using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Branch.Models.Sql
{
	public class BranchRole
		: Audit
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		
		[Required]
		public RoleType Type { get; set; }

		public virtual ICollection<BranchIdentity> BranchIdentities { get; set; } 
	}

	public enum RoleType
	{
		Banned = 0x00,
		User = 0x01,
		Moderator = 0x30,
		Administrator = 0x69
	}
}
