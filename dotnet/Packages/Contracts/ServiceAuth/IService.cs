using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceAuth;

namespace Branch.Packages.Contracts.ServiceAuth
{
	public interface IService
	{
		Task<ResGetHalo4Token> GetHalo4Token(ReqGetHalo4Token req);

		Task<ResGetXboxLiveToken> GetXboxLiveToken(ReqGetXboxLiveToken req);
	}
}
