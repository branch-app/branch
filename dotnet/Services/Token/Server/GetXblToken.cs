using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Token
{
	public partial class RpcServer : ITokenService
	{
		public readonly string GetXblTokenSchema = @"
			{
				""type"": ""object"",
				""additionalProperties"": false,

				""required"": [
					""ignore_cache""
				],

				""properties"": {
					""ignore_cache"": {
						""type"": ""bool""
					}
				}
			}
		";

		public Task<GetXblTokenResponse> GetXblToken(HttpContext ctx, GetTokenRequest req)
		{
			return _app.GetXblToken(ctx, req.IgnoreCache);
		}
	}
}
