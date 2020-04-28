using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Identity.Server
{
	public partial class RpcServer : IIdentityService
	{
		public readonly string GetXblIdentitySchema = @"
			{
				""type"": ""object"",
				""additionalProperties"": false,

				""required"": [
					""type"",
					""value""
				],

				""properties"": {
					""ignore_cache"": {
						""type"": ""boolean""
					},

					""type"": {
						""type"": ""string"",
						""enum"": [""gamertag"", ""xuid""]
					},

					""value"": {
						""type"": ""string"",
						""minLength"": 1
					},
				}
			}
		";

		public Task<GetXblIdentityRes> GetXblIdentity(HttpContext ctx, GetXblIdentityReq req)
		{
			return _app.GetXblIdentity(ctx, req);
		}
	}
}
