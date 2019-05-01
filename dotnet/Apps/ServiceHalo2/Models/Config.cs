using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Branch.Packages.Models.Common.Config;

namespace Branch.Apps.ServiceHalo2.Models
{
	public class Config
	{
		public WorkerService WorkerService { get; set; }
	}

	public class WorkerService
	{
		public string Region { get; set; }
		public string AccessKeyId { get; set; }
		public string SecretAccessKey { get; set; }
		public string QueueUrl { get; set; }
	}
}
