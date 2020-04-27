using System;

namespace Branch.Global.Contracts
{
	public interface ICacheInfo
	{
		DateTime CachedAt { get; set; }

		Nullable<DateTime> ExpiresAt { get; set; }

		bool IsFresh();

		bool IsFresh(DateTime date);
	}
}
