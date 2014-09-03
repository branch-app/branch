using System;
using System.Web.Mvc;
using Branch.App.Areas.Reach.Filters;
using Branch.App.Areas.Reach.Models;
using Branch.Core.Game.HaloReach.Enums;
using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Controllers
{
	public class HistoryController : Controller
	{
		// GET: /360/{gamertag}/Reach/History/{slug}
		// GET: /360/{gamertag}/Halo4/History/{slug}?{page}
		[ValidateReachServiceRecordFilter]
		[ValidateReachApiStatus]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord, string slug)
		{
			VariantClass variantClass;
			Enum.TryParse(slug, out variantClass);
			if (variantClass == VariantClass.Unknown)
				return RedirectToAction("Index", "History", new
				{
					area = "Reach", gamertag, slug = VariantClass.Competitive
				});

			var page = int.Parse(Request.QueryString["page"] ?? "0");
			if (page < 0) page = 0;
			var gameHistory = GlobalStorage.HReachManager.GetPlayerGameHistory(gamertag, variantClass, (uint) page);
			return View(new HistoryViewModel(serviceRecord, variantClass, (uint) page, gameHistory));
		}
	}
}