using System.Web.Mvc;

namespace Branch.App.Areas.Halo4
{
	public class Halo4AreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Halo4";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			var namespaces = new[] { "Branch.App.Areas.Halo4.Controllers" };

			context.MapRoute( "Halo4_Default", "Game/Halo4/", 
				new {controller = "Home", action = "Index"}, namespaces);

			// Stuff
			context.MapRoute("Halo4_Challenges", "Game/Halo4/Challenges/{slug}",
				new {controller = "Challenges", slug = "wargames", action = "Index"}, namespaces);
			context.MapRoute("Halo4_Playlists", "Game/Halo4/Playlists/{slug}",
				new {controller = "Playlists", slug = "wargames", action = "Index"}, namespaces);

			// Service Record
			context.MapRoute("Halo4_ServiceRecord", "360/{gamertag}/Halo4/", 
				new {controller = "ServiceRecord", action = "Index"}, namespaces);

			// Specializations
			context.MapRoute("Halo4_Specializations", "360/{gamertag}/Halo4/Specializations",
				new {controller = "Specializations", action = "Index"}, namespaces);

			// Game History
			context.MapRoute("Halo4_History", "360/{gamertag}/Halo4/History/{slug}",
				new {controller = "History", slug = "WarGames", action = "Index"}, namespaces);

			// Game
			context.MapRoute("Halo4_Game", "360/{gamertag}/Halo4/Game/{id}", 
				new {controller = "Game", action = "Index"}, namespaces);

			// Commendations
			context.MapRoute("Halo4_Commendations", "360/{gamertag}/Halo4/Commendations/{slug}",
				new {controller = "Commendations", slug = "Weapons", action = "Index"}, namespaces);

			// CSR
			context.MapRoute("Halo4_Csr", "360/{gamertag}/Halo4/Csr/", 
				new {controller = "Csr", action = "Index"}, namespaces);
			context.MapRoute("Halo4_CsrDetails", "360/{gamertag}/Halo4/Csr/{id}/{slug}",
				new {controller = "Csr", action = "Details"}, namespaces);
		}
	}
}
