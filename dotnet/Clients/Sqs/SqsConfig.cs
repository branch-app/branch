using System;

namespace Branch.Clients.Sqs
{
	public class SqsConfig
	{
		public string Region { get; set; }

		public string AccessKeyId { get; set; }

		public string SecretAccessKey { get; set; }

		public string QueueUrl { get; set; }
	}
}
