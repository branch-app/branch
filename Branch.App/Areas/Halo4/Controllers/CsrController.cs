using System.Linq;
using System.Web.Mvc;
using Branch.App.Areas.Halo4.Filters;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Helpers.Mvc;
using Branch.App.Helpers.Razor.Halo4;
using Branch.Core.Game.Halo4.Models._343.Responses;
using Branch.Extenders;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class CsrController : Controller
	{
		//
		// GET: /Halo4/{gamertag}/Csr/
		[ValidateH4ServiceRecordFilter]
		[ValidateH4ApiStatus]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord)
		{
			return View(new CsrViewModel(serviceRecord, GlobalStorage.H4Manager.GetPlaylistOrientations()));
		}

		//
		// GET: /Halo4/{gamertag}/Csr/{id}-{slug}
		[ValidateH4ServiceRecordFilter]
		[ValidateH4ApiStatus]
		public ActionResult Details(string gamertag, ServiceRecord serviceRecord, int? id, string slug)
		{
			if (id == null)
				return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Index", "Csr"),
					FlashMessage.FlashMessageType.Failure, "Playlist Error", "There was no specified playlist id, the url must have been malformed.");

			var skillRank = serviceRecord.SkillRanks.FirstOrDefault(r => r.PlaylistId == id);
			if (skillRank == null)
				return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Index", "Csr"),
					FlashMessage.FlashMessageType.Failure, "Playlist Error", "The specified playlist doesn't exist. It either was purged by 343, or never existed.");

			var playlist = MetadataHelpers.GetPlaylist(skillRank.PlaylistId);
			if (playlist == null)
				return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Index", "Csr"),
					FlashMessage.FlashMessageType.Failure, "Playlist Error", "The specified playlist doesn't exist. It either was purged by 343, or never existed. Try waiting for 343 to update their systems and try again in 30 minutes.");


			if (skillRank.PlaylistName.ToSlug() != slug)
				return FlashMessage.RedirectAndFlash(Response, RedirectToAction("Details", "Csr", new { id, slug = skillRank.PlaylistName.ToSlug() }),
					FlashMessage.FlashMessageType.Warning, "Haha!", "Nice try changing the slug m8.");

			return
				View(new CsrDetailViewModel(serviceRecord, GlobalStorage.H4Manager.GetPlaylistOrientation(skillRank.PlaylistId),
					serviceRecord.SkillRanks.FirstOrDefault(r => r.PlaylistId == id), MetadataHelpers.GetPlaylist(skillRank.PlaylistId)));
		}
	}
}
