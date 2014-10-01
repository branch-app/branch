using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Models
{
	public class GameViewModel
		: Base
	{
		public GameViewModel(ServiceRecord serviceRecord, Game game)
			: base(serviceRecord)
		{
			Game = game;
		}

		public Game Game { get; set; }
	}
}