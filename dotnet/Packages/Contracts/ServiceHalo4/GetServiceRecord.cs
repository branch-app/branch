using System;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Models.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceHalo4
{
	public class ReqGetServiceRecord
	{
		[JsonProperty("identity")]
		public IdentityRequest Identity { get; set; }
	}

	public class ResGetServiceRecord : ServiceRecordResponse, IBranchResponse
	{
		[JsonProperty("cache_info")]
		public ICacheInfo CacheInfo { get; set; }
	}
}
