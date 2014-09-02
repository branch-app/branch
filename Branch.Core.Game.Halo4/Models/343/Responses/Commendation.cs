using System.Collections.Generic;
using Branch.Core.Game.Halo4.Models._343.DataModels;

namespace Branch.Core.Game.Halo4.Models._343.Responses
{
	public class Commendation : Response
	{
		public IList<CommendationModels.Commendation> Commendations { get; set; }
	}
}
