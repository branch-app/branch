using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class GameViewModel : Base
	{
		public GameViewModel(ServiceRecord serviceRecord, ElapsedGame elapsedGame)
			: base(serviceRecord)
		{
			ElapsedGame = elapsedGame;
		}

		public ElapsedGame ElapsedGame { get; set; }
	}
}