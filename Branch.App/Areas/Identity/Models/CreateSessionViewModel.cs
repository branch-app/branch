using System.ComponentModel.DataAnnotations;

namespace Branch.App.Areas.Identity.Models
{
	public class CreateSessionViewModel
	{
		[Required(ErrorMessage = "You forgot {0}")]
		public string Username { get; set; }

		[Required(ErrorMessage = "You forgot {0}")]
		public string Password { get; set; }

		[Display(Name = "Remember Me")]
		public bool RememberMe { get; set; }
	}
}