using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Apollo.Models;
using Branch.Packages.Models.Common.Config;

namespace Branch.Apps.ServiceHalo4.Models
{
	public class Config : ConfigBase
	{
		public Dictionary<string, BranchServiceConfig> Services { get; set; }

		public S3Credentials S3 { get; set; }
	}
}
