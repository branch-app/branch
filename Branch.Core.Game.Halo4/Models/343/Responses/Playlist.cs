using System.Collections.Generic;
using Branch.Core.Game.Halo4.Models._343.DataModels;

namespace Branch.Core.Game.Halo4.Models._343.Responses
{
	public class Playlist : Response
	{
		public IList<PlaylistModel> Playlists { get; set; }
	}
}
