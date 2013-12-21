using Branch.Models.Services.Halo4._343.DataModels;
using System.Collections.Generic;

namespace Branch.Models.Services.Halo4._343.Responses
{
	public class GameHistory<T> : WaypointResponse
		where T : GameHistoryModel.Base
	{
		public int DateFidelity { get; set; }

		public List<T> Games { get; set; }
	}
}
