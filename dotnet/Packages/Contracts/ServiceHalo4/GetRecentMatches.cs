using System;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Enums.Halo4;
using Branch.Packages.Models.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceHalo4
{
	public class ReqGetRecentMatches
	{
		[JsonProperty("identity")]
		public IdentityRequest Identity { get; set; }

		[JsonProperty("game_mode")]
		public GameMode GameMode { get; set; }

		[JsonProperty("start_at")]
		public uint? StartAt { get; set; }

		[JsonProperty("count")]
		public uint? Count { get; set; }
	}

	public class ResGetRecentMatches : RecentMatchesResponse, IBranchResponse
	{
		[JsonProperty("cache_info")]
		public ICacheInfo CacheInfo { get; set; }
	}
}
