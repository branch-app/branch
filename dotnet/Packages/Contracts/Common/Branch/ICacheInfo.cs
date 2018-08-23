using System;

namespace Branch.Packages.Contracts.Common.Branch
{
	public interface ICacheInfo
	{
		
		DateTime CachedAt { get; set; }

		DateTime ExpiresAt { get; set; }
	}
}
