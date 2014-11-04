
using System.ComponentModel.DataAnnotations;

namespace Branch.Models.Sql
{
	public class Authentication
		: Audit
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public AuthenticationType Type { get; set; }

		[Required]
		public string Key { get; set; }

		[Required]
		public bool IsValid { get; set; }
	}

	public enum AuthenticationType
	{
		Halo4
	}
}
