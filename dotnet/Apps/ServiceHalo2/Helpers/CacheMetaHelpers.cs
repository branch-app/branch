using System;
using System.Threading.Tasks;
using Branch.Apps.ServiceHalo2.Database;
using Branch.Packages.Bae;

namespace Branch.Apps.ServiceHalo2.Helpers
{
	public static class CacheMetaHelpers
	{
		public static async Task<bool> NeedsQueueing(DatabaseClient client, string identifier)
		{
			var meta = await client.GetCacheMeta(identifier);

			switch(meta?.CacheState)
			{
				case null: return true;
				case "complete": return false;
				case "failed": throw meta.CacheFailure;
				case "in_progress": throw new BaeException("currently_caching");

				default:
					throw new InvalidOperationException("Yuuuge failure!");
			}
		}
	}
}
