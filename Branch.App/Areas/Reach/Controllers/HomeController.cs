using System.Web.Mvc;
using Branch.App.Areas.Reach.Filters;
using Branch.App.Areas.Reach.Models;
using Branch.Core.Game.HaloReach.Models.Branch;
using Branch.Models.Services.Branch;

namespace Branch.App.Areas.Reach.Controllers
{
	public class HomeController : Controller
	{
		//
		// GET: /360/Reach/
		[ValidateReachApiStatus]
		public ActionResult Index()
		{
			var total = GlobalStorage.AzureStorage.Table.QueryAndRetrieveCount<GamerIdEntity>("",
				GlobalStorage.AzureStorage.Table.BranchCloudTable);

			var reachTotal = GlobalStorage.AzureStorage.Table.QueryAndRetrieveCount<ServiceRecordEntity>("",
				GlobalStorage.AzureStorage.Table.HReachCloudTable);

			//var replacement =
			//storage.Table.QueryAndRetrieveSingleEntity<GamerIdReplacementEntity>(
			//	TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GamerIdReplacementEntity.PartitionKeyString),
			//	TableOperators.And,
			//	TableQuery.GenerateFilterConditionForInt("TypeInt", QueryComparisons.Equal, (int)gamerIdType),
			//	TableOperators.And,
			//	TableQuery.GenerateFilterCondition("SafeReplacementId", QueryComparisons.Equal, gamerId.ToLowerInvariant()),
			//	storage.Table.BranchCloudTable);

			return View(new HomeViewModel { TotalPlayers = total, TotalReachPlayers = reachTotal });
		}
	}
}