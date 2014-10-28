namespace Branch.Models.Services.Branch
{
	public class Pbkdf2Response
	{
		public int Iterations { get; set; }

		public string Salt { get; set; }

		public string Hash { get; set; }
	}
}
