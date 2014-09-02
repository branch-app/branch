using System.Collections.Generic;
using Branch.Core.Game.Halo4.Models._343.DataModels;

namespace Branch.Core.Game.Halo4.Models._343.Responses
{
	public class GameHistory<T> : Response
		where T : GameHistoryModel.Base
	{
		public int DateFidelity { get; set; }

		public List<T> Games { get; set; }
	}
}
