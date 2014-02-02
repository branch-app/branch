using Halo4 = Branch.Models.Services.Halo4._343.Responses;
namespace Branch.App.Models
{
	public class SearchViewModel
	{
		public SearchViewModel(string query, Halo4.ServiceRecord halo4ServiceRecord)
		{
			Query = query;
			Halo4ServiceRecord = halo4ServiceRecord;
		}

		public string Query { get; set; }

		public Halo4.ServiceRecord Halo4ServiceRecord { get; set; }
	}
}