using Branch.Core.Game.Halo4.Enums;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class PlaylistsViewModel
	{
		public PlaylistsViewModel(GameMode selectedMode, Playlist playlists)
		{
			SelectedMode = selectedMode;
			Playlists = playlists;
		}

		public GameMode SelectedMode { get; set; }

		public Playlist Playlists { get; set; }
	}
}