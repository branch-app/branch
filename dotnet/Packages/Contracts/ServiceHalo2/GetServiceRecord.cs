using System;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Models.Halo2;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceHalo2
{
	public class ReqGetServiceRecord
	{
		[JsonProperty("gamertag")]
		public string Gamertag { get; set; }
	}

	public class ResGetServiceRecord : ServiceRecordResponse, IBranchResponse
	{
		[JsonProperty("cache_info")]
		public ICacheInfo CacheInfo { get; set; }
	}
}
