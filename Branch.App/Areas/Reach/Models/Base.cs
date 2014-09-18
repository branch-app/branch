using Branch.Core.BranchStuff;
using Branch.Core.Game.HaloReach.Enums;
using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Models
{
	public class Base
	{
		public Base(ServiceRecord serviceRecord)
		{
			ServiceRecord = serviceRecord;
			RecentGamesHistory = GlobalStorage.HReachManager.GetPlayerGameHistory(ServiceRecord.Player.Gamertag, VariantClass.All);
			PublicGamertag = GamerIdReplacementManager.GetReplacementGamerId(ServiceRecord.Player.Gamertag,
				GlobalStorage.AzureStorage);
		}

		public GameHistory RecentGamesHistory { get; set; }

		public string PublicGamertag { get; set; }

		public ServiceRecord ServiceRecord { get; set; }
	}
}