using System;
using System.Collections.Generic;
using Branch.Models.Services.Halo4._343.DataModels;

namespace Branch.Models.Services.Halo4._343.Responses
{
	public class Challenge : WaypointResponse
	{
		public DateTime Date { get; set; }

		public List<ChallengeModel> Challenges { get; set; }

		public int TotalCompleted { get; set; }
	}
}
