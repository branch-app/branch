using Branch.Models.Services.Halo4._343.Responses;
using _Enums = Branch.Models.Services.Halo4._343.DataModels.Enums;

namespace Branch.App.Areas.Halo4.Models
{
	public class PlaylistsViewModel
	{
		public PlaylistsViewModel(_Enums.GameMode selectedMode, Playlist playlists)
		{
			SelectedMode = selectedMode;
			Playlists = playlists;
		}

		public _Enums.GameMode SelectedMode { get; set; }

		public Playlist Playlists { get; set; }
	}
}