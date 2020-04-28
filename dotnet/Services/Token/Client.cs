using System.Collections.Generic;
using System.Threading.Tasks;
using Branch.Global.Contracts;
using Branch.Global.Libraries;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Token
{
	public class TokenClient : ITokenService
	{
		private JsonClient _client { get; }

		public TokenClient(string baseUrl, string key)
		{
			var options = new HttpClientOptions
			{
				Headers = new Dictionary<string, string> { { "Authorization", $"bearer {key}" } },
			};

			_client = new JsonClient(baseUrl, options);
		}

		public async Task<GetXblTokenResponse> GetXblToken(HttpContext ctx, GetTokenRequest req)
		{
			return await _client.Do<GetXblTokenResponse>("POST", "1/preview/get_xbl_token", req);
		}
	}
}
