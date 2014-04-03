using Branch.Extenders;

namespace Branch.Models.Services.Branch
{
	public sealed class GamerIdReplacementEntity : BaseEntity
	{
		public static readonly string RowKeyString = "{0}";
		public static readonly string PartitionKeyString = "GamerIdReplacement";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gamerId"></param>
		/// <param name="replacementId"></param>
		/// <param name="gamerIdType"></param>
		public GamerIdReplacementEntity(string gamerId, string replacementId, GamerId gamerIdType)
		{
			Id = gamerId.ToLowerInvariant();
			OriginalId = gamerId;
			ReplacementId = replacementId;
			SafeReplacementId = replacementId.ToLowerInvariant();
			Type = gamerIdType;

			SetKeys(PartitionKeyString, string.Format(Id.ToSlug(), gamerId));
		}

		public GamerIdReplacementEntity() { }

		/// <summary>
		/// The escaped (ToLowered) version of the Gamers Id
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// The original version of the Gamers Id
		/// </summary>
		public string OriginalId { get; set; }

		/// <summary>
		/// The original version of the Gamers Replacement Id
		/// </summary>
		public string ReplacementId { get; set; }

		/// <summary>
		/// The escaped (ToLowered) version of the Gamers Replacement Id
		/// </summary>
		public string SafeReplacementId { get; set; }

		public int TypeInt { get; set; }

		public GamerId Type
		{
			get { return (GamerId) TypeInt; }
			set { TypeInt = (int) value; }
		}
	}
}
