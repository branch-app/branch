using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Token.App
{
	public partial class Application
	{
		public async Task<GetXblTokenResponse> GetXblToken(HttpContext ctx, bool ignoreCache)
		{
			return null;
		}
	}
}
