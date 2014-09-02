using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Metadata
	{
		[JsonProperty("AllCommendationsById")]
		public Commendation[] Commendations { get; set; }

		[JsonProperty("AllEnemiesById")]
		public Enemy[] Enemies { get; set; }

		[JsonProperty("AllMapsById")]
		public Map[] Maps { get; set; }

		[JsonProperty("AllMedalsById")]
		public Medal[] Medals { get; set; }

		[JsonProperty("AllPlaylistsById")]
		public Playlist[] Playlists { get; set; }

		[JsonProperty("AllWeaponsById")]
		public Weapon[] Weapons { get; set; }

		[JsonProperty("GameVariantClassesKeysAndValues")]
		public GameVariantClass[] GameVariantClasses { get; set; }

		[JsonProperty("GlobalRanksById")]
		public Rank[] Ranks { get; set; }
	}
}
