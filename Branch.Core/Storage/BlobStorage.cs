using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Branch.Core.Storage
{
	public class BlobStorage
	{
		public CloudBlobClient BlobClient { get; private set; }
		public CloudStorageAccount StorageAccount { get; private set; }

		public BlobStorage(CloudStorageAccount storage)
		{
			StorageAccount = storage;

			BlobClient = StorageAccount.CreateCloudBlobClient();

			#region Initialize Blobs

			H4BlobContainer = BlobClient.GetContainerReference("h4-blob-container");
			H4BlobContainer.CreateIfNotExists();
			H4BlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Off });

			#endregion
		}

		#region Blob Operations

		public void UploadBlob(CloudBlobContainer blobContainer, string blobName, string blobData)
		{
			var blob = blobContainer.GetBlockBlobReference(blobName);
			blob.UploadText(blobData);
		}
		public void UploadBlob(CloudBlobContainer blobContainer, string blobName, byte[] blobData)
		{
			var blob = blobContainer.GetBlockBlobReference(blobName);
			blob.UploadFromByteArray(blobData, 0, blobData.Length);
		}

		#endregion

		public CloudBlobContainer H4BlobContainer { get; private set; }
	}
}
