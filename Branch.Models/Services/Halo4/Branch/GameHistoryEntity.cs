using System;

namespace Branch.Models.Services.Halo4.Branch
{
	public class GameHistoryEntity : BaseEntity
	{
		public GameHistoryEntity(string ident)
		{
			SetKeys(null, ident);

			Ident = ident;
		}

		public string Ident { get; set; }

		public int Standing { get; set; }

		public int BaseVariantId { get; set; }
		
		public string VariantName { get; set; }

		public string FeaturedStatName { get; set; }

		public int FeaturedStatValue { get; set; }
		
		public int TotalMedals { get; set; }
		
		public int MapId { get; set; }

		public string MapVariantName { get; set; }

		public int PlayListId { get; set; }

		public string PlayListName { get; set; }

		public int PersonalScore { get; set; }

		public string Id { get; set; }

		public int ModeId { get; set; }

		public string ModeName { get; set; }

		public bool Completed { get; set; }

		public int Result { get; set; }

		public DateTime EndDateUtc { get; set; }

		#region Overrides

		public override sealed void SetKeys(string partitionKey, string rowKey)
		{
			base.SetKeys("GameHistory", rowKey);
		}

		#endregion
	}
}
