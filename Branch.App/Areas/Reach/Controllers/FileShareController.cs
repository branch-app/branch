using System.Web.Mvc;
using Branch.App.Areas.Reach.Filters;
using Branch.App.Areas.Reach.Models;
using Branch.App.Helpers;
using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Controllers
{
	public class FileShareController : BaseController
	{
		// GET: 360/{gamertag}/Reach/FileShare
		[ValidateReachApiStatus]
		[ValidateReachServiceRecordFilter]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord)
		{
			var fileShare = GlobalStorage.HReachManager.GetPlayerFileShare(gamertag);
			return View(new FileShareViewModel(serviceRecord, fileShare));
		}

		// GET: 360/{gamertag}/Reach/FileShare/File/{id}
		[ValidateReachApiStatus]
		[ValidateReachServiceRecordFilter]
		public ActionResult File(string gamertag, ServiceRecord serviceRecord, string fileId)
		{
			var fileShare = GlobalStorage.HReachManager.GetPlayerFile(long.Parse(fileId));
			return View(new FileShareFileViewModel(serviceRecord, fileShare));
		}

		// GET: 360/{gamertag}/Reach/FileShare/RecentScreenshots
		[ValidateReachApiStatus]
		[ValidateReachServiceRecordFilter]
		public ActionResult RecentScreenshots(string gamertag, ServiceRecord serviceRecord)
		{
			var fileShare = GlobalStorage.HReachManager.GetPlayersRecentScreenshots(gamertag);
			return View(new FileShareViewModel(serviceRecord, fileShare));
		}
	}
}
