using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Branch.Models.Sql
{
	public class BranchIdentity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Username { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		[Required]
		public string PasswordSalt { get; set; }

		[Required]
		public string PasswordIterations { get; set; }

		[Required]
		public string Email { get; set; }


		public virtual ICollection<BranchSession> BranchSessions { get; set; } 

		public virtual BranchRole BranchRole { get; set; }

		public virtual GamerIdentity GamerIdentity { get; set; }
	}
}
