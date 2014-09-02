using System.Collections.Generic;
using Branch.Core.Game.Halo4.Models._343.DataModels;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class GamePlayerListViewModel : Base
	{
		public GamePlayerListViewModel(ServiceRecord serviceRecord, IList<Game.Player> players, Game.Team team)
			: base(serviceRecord)
		{
			Players = players;
			Team = team;
		}

		public IList<Game.Player> Players { get; set; }

		public Game.Team Team { get; set; }
	}
}