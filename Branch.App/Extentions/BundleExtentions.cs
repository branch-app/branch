using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Optimization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Branch.App.Extentions
{
	public class AzureCdn
	{
		public static IHtmlString StylesRender(string bundleName)
		{
			var localPath = Styles.Render(bundleName).ToString();

#if DEBUG
			return new HtmlString(localPath);
#else
			return new HtmlString(localPath.Replace("/Content/", "http://cdn.branchapp.co/cdn/Content/"));
#endif
		}

		public static IHtmlString ScriptsRender(string bundleName)
		{
			var localPath = Scripts.Render(bundleName).ToString();
#if DEBUG
			return new HtmlString(localPath);
#else
			return new HtmlString(localPath.Replace("/Scripts/", "http://cdn.branchapp.co/cdn/Scripts/"));
#endif
		}
	}

	public class AzureScriptBundle : Bundle
	{
		public AzureScriptBundle(string virtualPath, string containerName, string cdnHost = "")
			: base(virtualPath, null, new IBundleTransform[] { new JsMinify(), new AzureBlobUpload { ContainerName = containerName, CdnHost = cdnHost } })
		{
			ConcatenationToken = ";";
		}
	}

	public class AzureStyleBundle : Bundle
	{
		public AzureStyleBundle(string virtualPath, string containerName, string cdnHost = "")
			: base(virtualPath, null, new IBundleTransform[] { new CssMinify(), new AzureBlobUpload { ContainerName = containerName, CdnHost = cdnHost } })
		{
		}
	}

	public class AzureBlobUpload : IBundleTransform
	{
		public string ContainerName { get; set; }

		public string CdnHost { get; set; }

		public virtual void Process(BundleContext context, BundleResponse response)
		{
			var file = VirtualPathUtility.GetFileName(context.BundleVirtualPath);

			if (!context.BundleCollection.UseCdn)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(ContainerName))
			{
				throw new Exception("ContainerName Not Set");
			}

			var connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

			var conn = CloudStorageAccount.Parse(connectionString);
			var cont = conn.CreateCloudBlobClient().GetContainerReference(ContainerName);
			cont.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
			var blob = cont.GetBlockBlobReference(file);

			blob.Properties.ContentType = response.ContentType;
			blob.UploadText(response.Content);

			var uri = string.IsNullOrWhiteSpace(CdnHost) ? blob.Uri.AbsoluteUri.Replace("http:", "").Replace("https:", "") : string.Format("{0}/{1}/{2}", CdnHost, ContainerName, file);

			using (var hashAlgorithm = CreateHashAlgorithm())
			{
				var hash = HttpServerUtility.UrlTokenEncode(hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(response.Content)));
				context.BundleCollection.GetBundleFor(context.BundleVirtualPath).CdnPath = string.Format("{0}?v={1}", uri, hash);
			}
		}

		private static SHA256 CreateHashAlgorithm()
		{
			if (CryptoConfig.AllowOnlyFipsAlgorithms)
			{
				return new SHA256CryptoServiceProvider();
			}

			return new SHA256Managed();
		}
	}

}