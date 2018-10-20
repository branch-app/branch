using System;

namespace Branch.Packages.Contracts.Common.Branch
{
	public class CacheInfo : ICacheInfo
	{
		public CacheInfo() { }

		/// <summary>
		/// Creates a new CacheInfo based on a cache time and an expiration time.
		/// </summary>
		/// <param name="cachedAt">The time the resource was cached.</param>
		/// <param name="expiresAt">The time the cached resource will expire.</param>
		public CacheInfo(DateTime cachedAt, DateTime expiresAt)
		{
			CachedAt = cachedAt;
			ExpiresAt = expiresAt;
		}

		/// <summary>
		/// Creates a new CacheInfo based on a cache time and an expiry length.
		/// </summary>
		/// <param name="cachedAt">The time the resource was cached.</param>
		/// <param name="cacheExpiration">How long until the resource will expire.</param>
		public CacheInfo(DateTime cachedAt, TimeSpan cacheExpiration)
		{
			CachedAt = cachedAt;
			ExpiresAt = CachedAt.Add(cacheExpiration);
		}

		/// <summary>
		/// Creates a new CacheInfo based on a resource that implements ICacheInfo
		/// </summary>
		/// <param name="cachedAt">The time the resource was cached.</param>
		/// <param name="cacheExpiration">How long until the resource will expire.</param>
		public CacheInfo(ICacheInfo cacheInfo)
		{
			CachedAt = cacheInfo.CachedAt;
			ExpiresAt = cacheInfo.ExpiresAt;
		}

		/// <summary>
		/// The time the resource was cached at.
		/// </summary>
		public DateTime CachedAt { get; set; }

		/// <summary>
		/// The time the resource expires.
		/// </summary>
		public DateTime ExpiresAt { get; set; }

		/// <summary>
		/// Checks if the data is still fresh right now.
		/// </summary>
		public bool IsFresh()
		{
			return IsFresh(DateTime.UtcNow);
		}

		/// <summary>
		/// Checks if the data is still fresh and hasn't expired base don a point in time.
		/// </summary>
		/// <param name="date">The time to check the content freshness against.</param>
		public bool IsFresh(DateTime date)
		{
			return ExpiresAt > date;
		}
	}
}
