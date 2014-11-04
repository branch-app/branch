using System.ComponentModel.DataAnnotations;

namespace Branch.App.Areas.Identity.Models
{
	public class CreateIdentityViewModel
	{
		[Required(ErrorMessage = "You need a {0}")]
		[MinLength(1, ErrorMessage = "The minimum length for {0} is {1}")]
		[MaxLength(25, ErrorMessage = "The maximium length for {0} is {1}")]
		[RegularExpression(@"\w+", ErrorMessage = "Invalid {0} format")]
		public string Username { get; set; }

		[Required(ErrorMessage = "You need a {0}")]
		[MinLength(7, ErrorMessage = "The minimum length for {0} is {1}")]
		[MaxLength(100, ErrorMessage = "The maximium length for {0} is {1}")]
		public string Password { get; set; }

		[Display(Name = "Password Confirmation")]
		public string PasswordConfirm { get; set; }

		[Required(ErrorMessage = "You need a {0}")]
		[RegularExpression(@"[\w\d\-.+]+@[\w\d\-.+]+\.[a-zA-Z]{2,25}", ErrorMessage = "Invalid {0} format")]
		public string Email { get; set; }

		[Display(Name = "Full Name")]
		[MinLength(3, ErrorMessage = "The minimum length for {0} is {1}")]
		[MaxLength(30, ErrorMessage = "The maximium length for {0} is {1}")]
		[RegularExpression(@"\A[A-Za-z\- ]+\z", ErrorMessage = "Invalid {0} format")]
		public string FullName { get; set; }

		[MinLength(1, ErrorMessage = "The minimum length for {0} is {1}")]
		[MaxLength(16, ErrorMessage = "The maximium length for {0} is {1}")]
		[RegularExpression(@"[a-zA-Z]([a-zA-Z0-9]{0,15} ?)*", ErrorMessage = "Invalid {0} format")]
		public string Gamertag { get; set; }

		[Display(Name = "Invitation Code")]
		[Required(ErrorMessage = "You need a {0}")]
		public string InvitationCode { get; set; }
	}
}