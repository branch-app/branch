namespace Branch.Models.Services.Halo4.Branch
{
	public sealed class BlobRecordEntity : BaseEntity
	{
		public const string RowKeyString = "H4Blob_{0}_{1}";

		public BlobRecordEntity(string ident, int blobTypeInt, string blobType)
		{
			Ident = ident;
			BlobTypeInt = blobTypeInt;

			SetKeys("H4BlobCache", string.Format(RowKeyString, blobType, Ident));
		}

		public string Ident { get; set; }

		public int BlobTypeInt { get; set; }
	}
}
