namespace Branch.Models.Services.Halo4.Branch
{
	public sealed class PlaylistOrientationEntity : BaseEntity
	{
		public static string RowKeyString = "PlaylistId_{0}";
		public static string PartitionKeyString = "H4PlaylistOrientation";

		public PlaylistOrientationEntity() { }

		public PlaylistOrientationEntity(string playlistName, int playlistId, bool isTeam)
		{
			PlaylistName = playlistName;
			PlaylistId = playlistId;
			IsTeam = isTeam;

			SetKeys(PartitionKeyString, string.Format(RowKeyString, playlistId));
		}

		public string PlaylistName { get; set; }

		public int PlaylistId { get; set; }

		public bool IsTeam { get; set; }
	}
}
