using Branch.Core.Game.HaloReach.Enums;
using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Models
{
	public class HistoryViewModel
		: Base
	{
		public HistoryViewModel(ServiceRecord serviceRecord, VariantClass variantClass, uint page, GameHistory gameHistory)
			: base(serviceRecord)
		{
			VariantClass = variantClass;
			Page = page;
			GameHistory = gameHistory;
		}

		public VariantClass VariantClass { get; set; }

		public GameHistory GameHistory { get; set; }

		public uint Page { get; set; }
	}
}