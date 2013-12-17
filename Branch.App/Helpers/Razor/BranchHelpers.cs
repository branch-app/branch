namespace Branch.App.Helpers.Razor
{
	public class BranchHelpers
	{
		public static string CheckGamertagPrivacy(string gamertag)
		{
			if (gamertag == "Doofette") return "Evie";
			return gamertag;
		}

		public static string NumberWithDelimiter(int number, string delimiter)
		{
			return number >= 10000 ? number.ToString("n0") : number.ToString("d");
		}
	}
}
