using System;
using System.ComponentModel.DataAnnotations;

namespace Branch.Models.Sql
{
	public class BranchSession
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public Guid Identifier { get; set; }

		[Required]
		public string Ip { get; set; }

		[Required]
		public string FriendlyLocation { get; set; }

		[Required]
		public string GpsLocation { get; set; }

		[Required]
		public string UserAgent { get; set; }

		[Required]
		public string Platform { get; set; }

		[Required]
		public string Browser { get; set; }

		[Required]
		public string Version { get; set; }

		[Required]
		public bool Expired { get; set; }

		public virtual BranchIdentity BranchIdentity { get; set; }
	}
}
