using System;
using Branch.Packages.Contracts.Common.Branch;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceHalo2
{
	public class ReqGetServiceRecord
	{
		[JsonProperty("gamertag")]
		public string Gamertag { get; set; }
	}

	public class ResGetServiceRecord : IBranchResponse
	{
		[JsonProperty("cache_info")]
		public ICacheInfo CacheInfo { get; set; }
	}
}
