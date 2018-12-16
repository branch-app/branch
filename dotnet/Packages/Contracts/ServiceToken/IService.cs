using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceToken;

namespace Branch.Packages.Contracts.ServiceToken
{
	public interface IService
	{
		Task<ResGetHalo4Token> GetHalo4Token(ReqGetHalo4Token req);

		Task<ResGetXboxLiveToken> GetXboxLiveToken(ReqGetXboxLiveToken req);
	}
}
