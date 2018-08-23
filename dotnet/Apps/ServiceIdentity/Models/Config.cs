using System.Collections.Generic;
using Branch.Packages.Models.Common.Config;
using Microsoft.Extensions.Options;

namespace Branch.Apps.ServiceIdentity.Models
{
	public class Config
	{
		public Dictionary<string, BranchServiceConfig> Services { get; set; }

		public string InternalKey { get; set; }
	}
}
