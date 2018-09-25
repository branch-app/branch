using System;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Enums.Halo4;
using Branch.Packages.Models.Halo4;
using Branch.Packages.Models.Halo4.RecentMatches;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceHalo4
{
	public class ReqGetRecentMatches
	{
		public IdentityRequest Identity { get; set; }

		public GameMode GameMode { get; set; }

		public uint? StartAt { get; set; }

		public uint? Count { get; set; }
	}

	public class ResGetRecentMatches : IBranchResponse
	{
		public ICacheInfo CacheInfo { get; set; }

		public RecentMatch[] Matches { get; set; }

		public bool HasMoreMatches { get; set; }

		public int DateFidelity { get; set; }
	}
}
