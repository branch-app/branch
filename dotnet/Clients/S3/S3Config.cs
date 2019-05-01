using System;

namespace Branch.Clients.S3
{
	public class S3Config
	{
		public string Region { get; set; }

		public string AccessKeyId { get; set; }

		public string SecretAccessKey { get; set; }

		public string BucketName { get; set; }
	}
}
