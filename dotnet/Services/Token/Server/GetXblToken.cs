using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Token.Server
{
	public partial class RpcServer : ITokenService
	{
		public readonly string GetXblTokenSchema = @"
			{
				""type"": ""object"",
				""additionalProperties"": false,

				""properties"": {
					""ignore_cache"": {
						""type"": ""boolean""
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
