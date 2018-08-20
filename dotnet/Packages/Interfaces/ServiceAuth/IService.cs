using System.Threading.Tasks;
using Branch.Packages.Models.ServiceAuth;

namespace Branch.Packages.Interfaces.ServiceAuth
{
	public interface IService
	{
		Task<ResGetHalo4Token> GetHalo4Token(ReqGetHalo4Token req);

		Task<ResGetXboxLiveToken> GetXboxLiveToken(ReqGetXboxLiveToken req);
	}
}
