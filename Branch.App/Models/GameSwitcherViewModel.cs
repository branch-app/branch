using Branch.App.Models.Enums;

namespace Branch.App.Models
{
	public class GameSwitcherViewModel
	{
		public GameSwitcherViewModel(string gamertag, Game activeGame)
		{
			Gamertag = gamertag;
			ActiveGame = activeGame;
		}

		public string Gamertag { get; set; }

		public Game ActiveGame { get; set; }
	}
}