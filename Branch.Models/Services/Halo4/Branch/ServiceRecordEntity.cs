using System;

namespace Branch.Models.Services.Halo4.Branch
{
	public class ServiceRecordEntity : BaseEntity
	{
		public ServiceRecordEntity(string gamertag)
		{
			SetKeys(null, gamertag);

			Gamertag = gamertag;
		}

		public string Gamertag { get; set; }

		public string ServiceTag { get; set; }

		public int SpartanPoints { get; set; }

		public DateTime LastPlayedUtc { get; set; }

		public DateTime FirstPlayedUtc { get; set; }

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

		#region Overrides

		public override sealed void SetKeys(string partitionKey, string rowKey)
		{
			base.SetKeys("ServiceRecord", rowKey);
		}

		#endregion
	}
}
