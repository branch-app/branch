using System.Collections.Generic;
using Branch.Models.Services.Halo4._343.DataModels;

namespace Branch.Models.Services.Halo4._343.Responses
{
	public class Commendation : WaypointResponse
	{
		public IList<CommendationModels.Commendation> Commendations { get; set; }
	}
}
