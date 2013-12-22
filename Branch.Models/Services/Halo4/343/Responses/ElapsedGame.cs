using Branch.Models.Services.Halo4._343.DataModels;

namespace Branch.Models.Services.Halo4._343.Responses
{
	public class ElapsedGame : WaypointResponse
	{
		public int DateFidelity { get; set; }

		public Game Game { get; set; } 
	}
}
