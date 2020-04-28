using System.Threading.Tasks;
using Branch.Global.Models.Domain;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Identity
{
	public interface IIdentityService
	{
		Task<GetXblIdentityRes> GetXblIdentity(HttpContext ctx, GetXblIdentityReq req);
	}

	public class GetXblIdentityReq
	{
		public bool IgnoreCache { get; set; }

		public string Type { get; set; }

		public string Value { get; set; }
	}

	public class GetXblIdentityRes : IBranchResponse
	{
		public CacheInfo CacheInfo { get; set; }

		public string Gamertag { get; set; }

		public string Xuid { get; set; }
	}
}
