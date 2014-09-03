using Branch.Models;

namespace Branch.Core.Game.HaloReach.Models.Branch
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

		#region Overrides

		public override sealed void SetKeys(string partitionKey, string rowKey)
		{
			base.SetKeys("ServiceRecord", rowKey);
		}

		#endregion
	}
}
