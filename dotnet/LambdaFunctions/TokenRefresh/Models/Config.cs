using System.Collections.Generic;
using Branch.Clients.Branch;
using Branch.Packages.Models.Common.Config;

namespace Branch.LambdaFunctions.TokenRefresh.Models
{
	public class Config
	{
		public Dictionary<string, BranchConfig> Services { get; set; }
	}
}
