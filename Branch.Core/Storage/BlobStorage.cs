using System;
using System.Diagnostics;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

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

		public ICloudBlob GetBlob(CloudBlobContainer blobContainer, string blobName)
		{
			try
			{
				return blobContainer.GetBlobReferenceFromServer(blobName);
			}
			catch (StorageException storageException)
			{
				Trace.TraceError(storageException.ToString());
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TDataModel"></typeparam>
		/// <param name="blobContainer"></param>
		/// <param name="blobName"></param>
		/// <returns></returns>
		public TDataModel FindAndDownloadBlob<TDataModel>(CloudBlobContainer blobContainer, string blobName)
			where TDataModel : class
		{
			ICloudBlob blob;
			try
			{
				blob = blobContainer.GetBlobReferenceFromServer(blobName);
			}
			catch (StorageException storageException)
			{
				Trace.TraceError(storageException.ToString());
				return null;
			}

			var blobData = new byte[blob.Properties.Length];
			blob.DownloadToByteArray(blobData, 0);
			var blobTextRepresentation = Encoding.ASCII.GetString(blobData);
			return JsonConvert.DeserializeObject<TDataModel>(blobTextRepresentation);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TDataModel"></typeparam>
		/// <param name="blob"></param>
		/// <returns></returns>
		public TDataModel DownloadBlob<TDataModel>(ICloudBlob blob)
			where TDataModel : class
		{
			if (blob == null)
				throw new ArgumentException("Can't download blob, specified blob is null.");

			var blobData = new byte[blob.Properties.Length];
			blob.DownloadToByteArray(blobData, 0);
			var blobTextRepresentation = Encoding.ASCII.GetString(blobData);
			return JsonConvert.DeserializeObject<TDataModel>(blobTextRepresentation);
		}

		public void UploadBlob(CloudBlobContainer blobContainer, string blobName, string blobData)
		{
			if (blobData == null)
				throw new ArgumentNullException("blobData", "BlobData in UploadBlob is broken.");

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
