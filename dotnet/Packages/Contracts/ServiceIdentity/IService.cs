using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceIdentity;

namespace Branch.Packages.Contracts.ServiceIdentity
{
	public interface IService
	{
		Task<ResGetXboxLiveIdentity> GetXboxLiveIdentity(ReqGetXboxLiveIdentity req);
	}
}
