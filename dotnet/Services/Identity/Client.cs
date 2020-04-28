using System.Collections.Generic;
using System.Threading.Tasks;
using Branch.Global.Libraries;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Identity
{
	public class IdentityClient : IIdentityService
	{
		private JsonClient _client { get; }

		public IdentityClient(string baseUrl, string key)
		{
			var options = new HttpClientOptions
			{
				Headers = new Dictionary<string, string> { { "Authorization", $"bearer {key}" } },
			};

			_client = new JsonClient(baseUrl, options);
		}

		public async Task<GetXblIdentityRes> GetXblIdentity(HttpContext ctx, GetXblIdentityReq req)
		{
			return await _client.Do<GetXblIdentityRes>("POST", "1/preview/get_xbl_identity", req);
		}
	}
}
