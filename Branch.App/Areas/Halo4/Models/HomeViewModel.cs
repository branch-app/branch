using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class HomeViewModel
	{
		public HomeViewModel(Challenge challenges, Playlist playlists)
		{
			Challenges = challenges;
			Playlists = playlists;
		}

		public Challenge Challenges { get; set; }

		public Playlist Playlists { get; set; }
	}
}