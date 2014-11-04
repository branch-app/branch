using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Branch.Models.Sql
{
	public class BranchIdentityInvitation
		: Audit
	{
		public BranchIdentityInvitation()
		{
			InvitationCode = Guid.NewGuid().ToString();
			Used = false;
		}

		public BranchIdentityInvitation(string inviteCode)
		{
			InvitationCode = inviteCode;
			Used = false;
		}

		[Key]
		public int Id { get; set; }

		[Required]
		public bool Used { get; set; }

		[Required]
		public string InvitationCode { get; set; }

		public virtual ICollection<BranchIdentity> BranchIdentities { get; set; }
	}
}
