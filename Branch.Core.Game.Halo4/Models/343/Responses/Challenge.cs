using System;
using System.Collections.Generic;
using Branch.Core.Game.Halo4.Models._343.DataModels;

namespace Branch.Core.Game.Halo4.Models._343.Responses
{
	public class Challenge : Response
	{
		public DateTime Date { get; set; }

		public List<ChallengeModel> Challenges { get; set; }

		public int TotalCompleted { get; set; }
	}
}
