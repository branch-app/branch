using System.Web.Mvc;
using Branch.App.Areas.Halo4.Filters;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Helpers;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class SpecializationsController : BaseController
	{
		//
		// GET: /Halo4/{gamertag}/Specializations/
		[ValidateH4ServiceRecordFilter]
		[ValidateH4ApiStatus]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord)
		{
			return View(new SpecializationsViewModel(serviceRecord));
		}
	}
}