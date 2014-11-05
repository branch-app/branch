using System;
using System.Web.Mvc;
using Branch.App.Areas.Reach.Filters;
using Branch.App.Areas.Reach.Models;
using Branch.App.Helpers;
using Branch.Core.Game.HaloReach.Enums;
using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Controllers
{
	public class CommendationsController : BaseController
	{
		// GET: 360/{gamertag}/commendations/{slug}
		[ValidateReachApiStatus]
		[ValidateReachServiceRecordFilter]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord, string slug)
		{
			CommendationVariantClass commendationVariantClass;
			Enum.TryParse(slug, true, out commendationVariantClass);
			return View(new CommendationsViewModel(serviceRecord, commendationVariantClass));
		}
	}
}