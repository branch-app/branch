using System;
using System.Collections.Generic;
using Branch.Packages.Contracts.Common.Branch;
using Microsoft.Extensions.Options;

namespace Branch.Apps.ServiceIdentity.Models
{
	public class XboxLiveIdentity : ICacheInfo
	{
		public string Gamertag { get; set; }

		public long XUID { get; set; }

		public DateTime CachedAt { get; set; }
		
		public DateTime ExpiresAt { get; set; }
	}
}
