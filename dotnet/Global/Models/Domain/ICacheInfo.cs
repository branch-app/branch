using System;

namespace Branch.Global.Models.Domain
{
	public interface ICacheInfo
	{
		DateTime CachedAt { get; set; }

		Nullable<DateTime> ExpiresAt { get; set; }

		bool IsFresh();

		bool IsFresh(DateTime date);
	}
}
