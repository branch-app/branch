using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Identity.App
{
	public partial class Application
	{
		private readonly string XblRedisKey = "identity:xbox-live";

		public async Task<GetXblIdentityRes> GetXblIdentity(HttpContext ctx, GetXblIdentityReq req)
		{
			var useCache = !req.IgnoreCache;

			using (var client = _redisClientsManager.GetClient())
			{
				return new GetXblIdentityRes
				{
					CacheInfo = null,
				};
			}
		}
	}
}
