using System.Collections.Generic;
using Branch.Packages.Models.Common.Config;

namespace Branch.LambdaFunctions.AuthRefresh.Models
{
	public class Config
	{
		public Dictionary<string, BranchServiceConfig> Services { get; set; }
	}
}
