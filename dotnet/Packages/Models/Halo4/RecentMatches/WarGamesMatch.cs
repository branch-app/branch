using System;
using Branch.Packages.Converters;
using Branch.Packages.Enums.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.RecentMatches
{
	public class RecentWarGamesMatch : RecentCustomGamesMatch
	{
		public WarGamesPlaylist Playlist { get; set; }
	}
}
