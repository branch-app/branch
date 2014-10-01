using Branch.Core.Game.HaloReach.Models._343.DataModels;

namespace Branch.App.Areas.Reach.Models
{
	public class GameDetailsTeamViewModel
	{
		public GameDetailsTeamViewModel(GameViewModel gameViewModel, PlayerGameDetails[] players, Team team)
		{
			GameViewModel = gameViewModel;
			Players = players;
			Team = team;
		}

		public GameViewModel GameViewModel { get; set; }

		public PlayerGameDetails[] Players { get; set; }

		public Team Team { get; set; }
	}
}