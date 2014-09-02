using System.Collections.Generic;
using Branch.Core.Game.Halo4.Enums;
using Branch.Core.Game.Halo4.Models._343.DataModels;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class CommendationsViewModel : Base
	{
		public CommendationsViewModel(ServiceRecord serviceRecord, IList<CommendationModels.Commendation> commendations, CommendationCategory commendationCategory)
			: base (serviceRecord)
		{
			Commendations = commendations;
			CommendationCategory = commendationCategory;
		}

		public IList<CommendationModels.Commendation> Commendations { get; set; }

		public CommendationCategory CommendationCategory { get; set; }
	}
}