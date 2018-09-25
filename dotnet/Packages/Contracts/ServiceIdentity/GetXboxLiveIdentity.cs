using System;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Contracts.Common.Branch;
using Newtonsoft.Json;
using Branch.Packages.Models.Common.XboxLive;

namespace Branch.Packages.Contracts.ServiceIdentity
{
	public class ReqGetXboxLiveIdentity
	{
		[JsonProperty("type")]
		public XboxLiveIdentityType Type { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }
	}

	public class ResGetXboxLiveIdentity : IBranchResponse
	{
		public ICacheInfo CacheInfo { get; set; }

		public string Gamertag { get; set; }

		public long XUID { get; set; }

		public Identity ToIdentity()
		{
			return new Identity
			{
				Gamertag = Gamertag,
				XUID = XUID,
			};
		}
	}
}
