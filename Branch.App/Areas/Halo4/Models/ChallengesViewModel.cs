using System.Collections.Generic;
using Branch.Core.Game.Halo4.Models._343.DataModels;

namespace Branch.App.Areas.Halo4.Models
{
	public class ChallengesViewModel
	{
		public ChallengesViewModel(MetadataModels.ChallengeCategory selectedChallengeCategory, List<MetadataModels.ChallengeCategory> challengeCategories, List<ChallengeModel> challenges)
		{
			SelectedChallengeCategory = selectedChallengeCategory;
			ChallengeCategories = challengeCategories;
			Challenges = challenges;
		}

		public MetadataModels.ChallengeCategory SelectedChallengeCategory { get; set; }

		public List<MetadataModels.ChallengeCategory> ChallengeCategories { get; set; }

		public List<ChallengeModel> Challenges { get; set; }
	}
}