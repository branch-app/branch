using System.Collections.Generic;
using Branch.Models.Services.Halo4._343.DataModels;

namespace Branch.Models.Services.Halo4._343.Responses
{
	public class GameHistory : WaypointResponse
	{
		public int DateFidelity { get; set; }

		public List<GameHistoryModel> Games { get; set; }
	}
}
