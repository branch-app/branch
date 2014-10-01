using System.Web.Mvc;
using Branch.Core.Game.HaloReach.Enums;

namespace Branch.App.Areas.Reach
{
	public class ReachAreaRegistration : AreaRegistration
	{
		public override string AreaName { get { return "Reach"; } }

		public override void RegisterArea(AreaRegistrationContext context)
		{
			var namespaces = new[] { "Branch.App.Areas.Reach.Controllers" };

			// Service Record
			context.MapRoute("Reach_ServiceRecord", "360/{gamertag}/Reach/",
				new { controller = "ServiceRecord", action = "Index" }, 
				namespaces);

			// Game History
			context.MapRoute("Reach_History", "360/{gamertag}/Reach/History/{slug}",
				new { controller = "History", action = "Index", slug = VariantClass.Competitive.ToString() },
				namespaces);

			// Game
			context.MapRoute("Reach_Game", "360/{gamertag}/Reach/Game/{id}",
				new { controller = "Game", action = "Index" }, namespaces);

			// Commendations
			context.MapRoute("Reach_Commendations", "360/{gamertag}/Reach/Commendations/{slug}",
				new { controller = "Commendations", action = "Index", slug = CommendationVariantClass.Multiplayer.ToString() },
				namespaces);

			// File Share
			context.MapRoute("Reach_FileShare", "360/{gamertag}/Reach/FileShare",
				new { controller = "FileShare", action = "Index" },
				namespaces);
			context.MapRoute("Reach_File", "360/{gamertag}/Reach/FileShare/File/{fileId}",
				new { controller = "FileShare", action = "File" },
				namespaces);
			context.MapRoute("Reach_RecentScreenshots", "360/{gamertag}/Reach/FileShare/RecentScreenshots",
				new { controller = "FileShare", action = "RecentScreenshots" },
				namespaces);
		}
	}
}