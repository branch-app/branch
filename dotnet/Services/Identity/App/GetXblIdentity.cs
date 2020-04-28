using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Identity.App
{
	public partial class Application
	{
		private readonly string XblRedisKey = "identity:xbox-live";

		public async Task<GetXblIdentityRes> GetXblIdentity(HttpContext ctx, GetXblIdentityReq req)
		{
			if (req.Type == "gamertag")
				return await _xblIdentityCache.GetByGamertag(req.Value, req.IgnoreCache);

			return await _xblIdentityCache.GetByGamertag(req.Value, req.IgnoreCache);
		}
	}
}
