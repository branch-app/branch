using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Token
{
	public interface ITokenService
	{
		Task<GetXblTokenResponse> GetXblToken(HttpContext ctx, GetTokenRequest req);
	}

	public class GetTokenRequest
	{
		public bool IgnoreCache { get; set; }
	}

	public class GetXblTokenResponse
	{

	}
}
