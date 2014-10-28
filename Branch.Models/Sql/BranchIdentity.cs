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
		public int PasswordIterations { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string FullName { get; set; }

		public virtual ICollection<BranchSession> BranchIdentitySessions { get; set; } 

		public virtual BranchRole BranchRole { get; set; }

		public virtual GamerIdentity GamerIdentity { get; set; }
	}
}
