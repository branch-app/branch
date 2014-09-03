using System.Web.Mvc;
using Branch.App.Areas.Reach.Filters;
using Branch.App.Areas.Reach.Models;
using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Controllers
{
	public class ServiceRecordController : Controller
	{
		//
		// GET: /360/{gamertag}/Halo4/
		// GET: /360/{gamertag}/Halo4/ServiceRecord/
		[ValidateReachApiStatus]
		[ValidateReachServiceRecordFilter]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord)
		{
			return View(new ServiceRecordViewModel(serviceRecord));
		}
	}
}
