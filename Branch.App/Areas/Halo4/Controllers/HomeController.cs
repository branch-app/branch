using System.Web.Mvc;
using Branch.App.Areas.Halo4.Models;
using Branch.Models.Services.Branch;
using _Enums = Branch.Models.Services.Branch.Enums;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class HomeController : Controller
	{
		//
		// GET: /Halo4/
		public ActionResult Index()
		{
			var weeklyStats =
				GlobalStorage.AzureStorage.Table.RetrieveSingleEntity<Halo4StatsEntity>(Halo4StatsEntity.PartitionKeyString,
					Halo4StatsEntity.FormatRowKey(_Enums.Halo4StatType.Weekly.ToString()),
					GlobalStorage.AzureStorage.Table.BranchCloudTable);

			var allTimeStats =
				GlobalStorage.AzureStorage.Table.RetrieveSingleEntity<Halo4StatsEntity>(Halo4StatsEntity.PartitionKeyString,
					Halo4StatsEntity.FormatRowKey(_Enums.Halo4StatType.AllTime.ToString()),
					GlobalStorage.AzureStorage.Table.BranchCloudTable);

			return View(new HomeViewModel(weeklyStats, allTimeStats));
		}
	}
}