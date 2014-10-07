using Branch.Extenders;

namespace Branch.Models.Services.Branch
{
	public sealed class GamerIdEntity : BaseEntity
	{
		public static readonly string RowKeyString = "{0}";
		public static readonly string PartitionKeyString = "GamerId";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gamerId"></param>
		/// <param name="gamerIdType"></param>
		public GamerIdEntity(string gamerId, GamerId gamerIdType)
		{
			Id = gamerId;
			Type = gamerIdType;

			SetKeys(PartitionKeyString, string.Format(RowKeyString, Id.ToSlug().ToLowerInvariant()));
		}

		public GamerIdEntity() { }

		public string Id { get; set; }

		public int TypeInt { get; set; }

		public GamerId Type
		{
			get { return (GamerId) TypeInt; }
			set { TypeInt = (int) value; }
		}
	}
}
