using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;

namespace Branch.Core.Storage
{
	public class AzureStorage
	{
		public BlobStorage Blob { get; private set; }
		public TableStorage Table { get; private set; }
		public CloudStorageAccount StorageAccount { get; private set; }

		public AzureStorage()
		{
// ReSharper disable once RedundantAssignment
			var connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

#if DEBUG
			connectionString = "UseDevelopmentStorage=true";
#endif

			StorageAccount = CloudStorageAccount.Parse(connectionString);

			Blob = new BlobStorage(StorageAccount);
			Table = new TableStorage(StorageAccount);
		}
	}
}
