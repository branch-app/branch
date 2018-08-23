using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceAuth;

namespace Branch.Packages.Contracts.ServiceAuth
{
	public interface IService
	{
		Task<ResGetHalo4Token> GetHalo4Token();

		Task<ResGetXboxLiveToken> GetXboxLiveToken();
	}
}
