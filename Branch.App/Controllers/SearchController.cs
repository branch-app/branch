using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Branch.App.Helpers.Mvc;
using Branch.App.Models;
using Branch.App.Models.Enums;
using Branch.Core.Storage;
using Branch.Models.Sql;

namespace Branch.App.Controllers
{
	public class SearchController : Controller
	{
		//
		// GET: /Search/?q=
		public ActionResult Index(string q)
		{
			if (String.IsNullOrEmpty(q))
				return RedirectToAction("Index", "Home");

			using (var sqlStorage = new SqlStorage())
			{
				GlobalStorage.H4Manager.GetPlayerServiceRecord(q, true);
				GlobalStorage.HReachManager.GetPlayerServiceRecord(q, true);

				var halo4Identities =
					sqlStorage.Halo4Identities.Include(i => i.GamerIdentity)
						.Where(h => h.GamerIdentity.GamerId.Contains(q))
						.Take(6)
						.ToList();

				var reachIdentnties =
					sqlStorage.ReachIdentities.Include(i => i.GamerIdentity)
					.Where(h => h.GamerIdentity.GamerId.Contains(q))
					.Take(6)
					.ToList();

				// le render le model le
				return View(new SearchViewModel(q, halo4Identities, reachIdentnties));
			}
		}

		//
		// GET: /Search/Identity/{ident}?q={q}&page={page:0}
		public ActionResult Identity(string ident, string q, int? page = 0)
		{
			if (page == null || page < 0)
				return RedirectToAction("Identity", "Search", new { ident, q, page = 0 });

			if (String.IsNullOrEmpty(q))
				return FlashMessage.RedirectAndFlash(
					Response, RedirectToAction("Index", "Home"), FlashMessage.FlashMessageType.Info,
					"Missing Search Term", "Ey man, there was no search term there.");

			if (String.IsNullOrWhiteSpace(ident))
				return RedirectToAction("Index", "Search", new { q });

			SearchIdent searchIdent;
			if (!Enum.TryParse(ident, true, out searchIdent))
				return FlashMessage.RedirectAndFlash(
					Response, RedirectToAction("Index", "Search"), FlashMessage.FlashMessageType.Info,
					"Invalid Search Identity", "Now Now Bae.. You can't be searchin' like that.");

			using (var sqlStorage = new SqlStorage())
			{
				List<Halo4Identity> halo4Identities = null;
				List<ReachIdentity> reachIdentities = null;

				bool hasMorePages;
				switch (searchIdent)
				{
					case SearchIdent.Halo4:
						GlobalStorage.H4Manager.GetPlayerServiceRecord(q, true);
						halo4Identities = sqlStorage.Halo4Identities
							.Include(i => i.GamerIdentity)
							.Where(i => i.GamerIdentity.GamerId.Contains(q))
							.OrderBy(i => i.GamerIdentity.GamerId)
							.Skip(12*((int) page))
							.Take(13)
							.ToList();

						hasMorePages = halo4Identities.Count() == 13;
						if (hasMorePages) halo4Identities = halo4Identities.Take(12).ToList();
						break;

					case SearchIdent.Reach:
						GlobalStorage.HReachManager.GetPlayerServiceRecord(q, true);
						reachIdentities = sqlStorage.ReachIdentities
							.Include(i => i.GamerIdentity)
							.Where(i => i.GamerIdentity.GamerId.Contains(q))
							.OrderBy(i => i.GamerIdentity.GamerId)
							.Skip(12*((int) page))
							.Take(13)
							.ToList();

						hasMorePages = reachIdentities.Count() == 13;
						if (hasMorePages) reachIdentities = reachIdentities.Take(12).ToList();
						break;

					default:
						return RedirectToAction("Index", "Search", new {q});
				}

				return View("Identity", new SearchIdentityViewModel(searchIdent, halo4Identities, reachIdentities, q, (int) page, hasMorePages));
			}
		}
	}
}
