using System;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Enums.Halo4;
using Branch.Packages.Models.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceHalo4
{
	public class ReqGetPlayerOverview
	{
		[JsonProperty("identity")]
		public IdentityRequest Identity { get; set; }
	}

	public class ResGetPlayerOverview : PlayerOverviewResponse, IBranchResponse
	{
		[JsonProperty("cache_info")]
		public ICacheInfo CacheInfo { get; set; }
	}
}
