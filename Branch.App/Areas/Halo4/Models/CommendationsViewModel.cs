using System.Collections.Generic;
using Branch.Models.Services.Halo4._343.DataModels;
using Branch.Models.Services.Halo4._343.Responses;
using _343Enums = Branch.Models.Services.Halo4._343.DataModels.Enums;

namespace Branch.App.Areas.Halo4.Models
{
	public class CommendationsViewModel : Base
	{
		public CommendationsViewModel(ServiceRecord serviceRecord, IList<CommendationModels.Commendation> commendations, _343Enums.CommendationCategory commendationCategory)
			: base (serviceRecord)
		{
			Commendations = commendations;
			CommendationCategory = commendationCategory;
		}

		public IList<CommendationModels.Commendation> Commendations { get; set; }

		public _343Enums.CommendationCategory CommendationCategory { get; set; }
	}
}