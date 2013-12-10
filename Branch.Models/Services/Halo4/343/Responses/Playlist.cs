using System.Collections.Generic;
using Branch.Models.Services.Halo4._343.DataModels;

namespace Branch.Models.Services.Halo4._343.Responses
{
	public class Playlist : WaypointResponse
	{
		public IList<PlaylistModel> Playlists { get; set; }
	}
}
