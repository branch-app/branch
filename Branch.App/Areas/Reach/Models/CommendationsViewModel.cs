using Branch.Core.Game.HaloReach.Enums;
using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Models
{
	public class CommendationsViewModel
		: Base
	{
		public CommendationsViewModel(ServiceRecord serviceRecord, CommendationVariantClass commendationVariantClass)
			: base(serviceRecord)
		{
			CommendationVariantClass = commendationVariantClass;
		}

		public CommendationVariantClass CommendationVariantClass { get; set; }
	}
}