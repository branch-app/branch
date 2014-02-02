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
		public GamerIdEntity(string gamerId, Enums.GamerId gamerIdType)
		{
			Id = gamerId;
			Type = gamerIdType;

			SetKeys(PartitionKeyString, string.Format(Id.ToSlug(), gamerId));
		}

		public string Id { get; set; }

		public int TypeInt { get; set; }

		public Enums.GamerId Type
		{
			get { return (Enums.GamerId) TypeInt; }
			set { TypeInt = (int) value; }
		}
	}
}
