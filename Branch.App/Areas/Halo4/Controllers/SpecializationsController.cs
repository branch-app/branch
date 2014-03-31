using System.Web.Mvc;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Filters;
using Branch.Models.Services.Halo4._343.Responses;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class SpecializationsController : Controller
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