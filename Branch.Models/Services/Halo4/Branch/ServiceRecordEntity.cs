namespace Branch.Models.Services.Halo4.Branch
{
	public class ServiceRecordEntity : BaseEntity
	{
		public ServiceRecordEntity() {  }

		public ServiceRecordEntity(string gamertag)
		{
			SetKeys(null, gamertag.ToLowerInvariant());

			Gamertag = gamertag;
		}

		public string Gamertag { get; set; }

		public string ServiceTag { get; set; }

		public int SpartanPoints { get; set; }

		public int TotalGamesStarted { get; set; }

		public int TotalMedalsEarned { get; set; }

		public int TotalChallengesCompleted { get; set; }

		public int Xp { get; set; }

		public int RankId { get; set; }

		public string RankName { get; set; }

		public int FavoriteWeaponId { get; set; }

		public string FavoriteWeaponName { get; set; }

		public string FavoriteWeaponDescription { get; set; }

		public int FavoriteWeaponTotalKills { get; set; }

		#region Global Branch Stuff

		public int WarGamesKills { get; set; }

		public int WarGamesDeaths { get; set; }

		public int WarGamesMedals { get; set; }

		public int WarGamesGames { get; set; }

		public string WarGamesDuration { get; set; }

		#endregion

		#region Overrides

		public override sealed void SetKeys(string partitionKey, string rowKey)
		{
			base.SetKeys("ServiceRecord", rowKey);
		}

		#endregion
	}
}
