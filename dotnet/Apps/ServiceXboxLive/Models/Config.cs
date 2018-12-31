using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Apollo.Models;
using Branch.Packages.Models.Common.Config;

namespace Branch.Apps.ServiceXboxLive.Models
{
	public class Config : BaseConfig
	{
		public Dictionary<string, BranchServiceConfig> Services { get; set; }
	}
}
