using System;
using System.Linq;
using System.Web.Mvc;
using Branch.App.Areas.Halo4.Models;
using Branch.App.Filters;
using Branch.App.Helpers.Razor.Halo4;
using Branch.Extenders;
using Branch.Models.Services.Halo4._343.Responses;

namespace Branch.App.Areas.Halo4.Controllers
{
	public class CsrController : Controller
	{
		//
		// GET: /Halo4/{gamertag}/Csr/
		[ValidateH4ServiceRecordFilter]
		public ActionResult Index(string gamertag, ServiceRecord serviceRecord)
		{
			return View(new CsrViewModel(serviceRecord, GlobalStorage.H4WaypointManager.GetPlaylistOrientations()));
		}

		//
		// GET: /Halo4/{gamertag}/Csr/{id}-{slug}
		[ValidateH4ServiceRecordFilter]
		public ActionResult Details(string gamertag, ServiceRecord serviceRecord, int? id, string slug)
		{
			if (id == null) throw new ArgumentException("Uh Oh, playlist Id is null.");

			var skillRank = serviceRecord.SkillRanks.FirstOrDefault(r => r.PlaylistId == id);
			if (skillRank == null) throw new ArgumentException("Uh Oh, playlist Id is not valid.");
			var playlist = MetadataHelpers.GetPlaylist(skillRank.PlaylistId);
			if (playlist == null) throw new ArgumentException("Unknown Playlist. Wait wait for 343 to update their systems");


			if (skillRank.PlaylistName.ToSlug() != slug)
				throw new ArgumentException("hue hue, nice try, don't change the slug u arse.");

			return
				View(new CsrDetailViewModel(serviceRecord, GlobalStorage.H4WaypointManager.GetPlaylistOrientation(skillRank.PlaylistId),
					serviceRecord.SkillRanks.FirstOrDefault(r => r.PlaylistId == id), MetadataHelpers.GetPlaylist(skillRank.PlaylistId)));
		}
	}
}
