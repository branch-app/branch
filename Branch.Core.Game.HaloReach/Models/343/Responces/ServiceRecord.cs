using Branch.Core.Game.HaloReach.Models._343.DataModels;

namespace Branch.Core.Game.HaloReach.Models._343.Responces
{
	public class ServiceRecord
		: Response
	{
		public AiPlaylistStatistics[] AiStatistics { get; set; }

		public Player Player { get; set; }

		public string PlayerEmblemUrl { get; set; }

		public string PlayerModelUrl { get; set; }

		public string PlayerModelUrlHiRes { get; set; }

		public PlaylistStatistics[] StatisticsByPlaylist { get; set; }
	}
}
