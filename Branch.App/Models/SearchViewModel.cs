using Halo4 = Branch.Core.Game.Halo4.Models._343.Responses;
using HaloReach = Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Models
{
	public class SearchViewModel
	{
		public SearchViewModel(string query, Halo4.ServiceRecord halo4ServiceRecord,
			HaloReach.ServiceRecord haloReachServiceRecord)
		{
			Query = query;
			Halo4ServiceRecord = halo4ServiceRecord;
			HaloReachServiceRecord = haloReachServiceRecord;

			if (Halo4ServiceRecord != null)
				Query = Halo4ServiceRecord.Gamertag;
			else if (HaloReachServiceRecord != null)
				Query = HaloReachServiceRecord.Player.Gamertag;
		}

		public string Query { get; set; }

		public Halo4.ServiceRecord Halo4ServiceRecord { get; set; }

		public HaloReach.ServiceRecord HaloReachServiceRecord { get; set; }
	}
}