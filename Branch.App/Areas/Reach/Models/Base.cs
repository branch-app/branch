using Branch.Core.BranchStuff;
using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Models
{
	public class Base
	{
		public Base(ServiceRecord serviceRecord)
		{
			ServiceRecord = serviceRecord;
			PublicGamertag = GamerIdReplacementManager.GetReplacementGamerId(ServiceRecord.Player.Gamertag, GlobalStorage.AzureStorage);
		}

		public string PublicGamertag { get; set; }

		public ServiceRecord ServiceRecord { get; set; }
	}
}