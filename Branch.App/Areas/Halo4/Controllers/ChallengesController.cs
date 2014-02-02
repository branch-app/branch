using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Branch.App.Helpers.Razor.Halo4;
using Branch.Models.Services.Halo4._343.DataModels;
using Microsoft.Ajax.Utilities;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class ChallengesController : Controller
	{
		//
		// GET: /Halo4/Challenges/
		public ActionResult Index()
		{
			var challenges = GlobalStorage.H4WaypointManager.Challenges.Challenges;
			var challengeCategories = new List<MetadataModels.ChallengeCategory>();

			foreach (var challenge in from challenge in challenges
				let weHaveIt = (challengeCategories.FirstOrDefault(c => c.Id == (int) challenge.Category) != null)
				where !weHaveIt
				select challenge)
			{
				challengeCategories.Add(MetadataHelpers.GetChallengeCategory((int)challenge.Category));
			}

			return View();
		}
	}
}