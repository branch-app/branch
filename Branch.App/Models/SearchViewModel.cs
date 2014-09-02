using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Models
{
	public class SearchViewModel
	{
		public SearchViewModel(string query, ServiceRecord halo4ServiceRecord)
		{
			Query = query;
			Halo4ServiceRecord = halo4ServiceRecord;
		}

		public string Query { get; set; }

		public ServiceRecord Halo4ServiceRecord { get; set; }
	}
}