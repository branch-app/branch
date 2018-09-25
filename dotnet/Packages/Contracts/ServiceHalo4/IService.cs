using System.Threading.Tasks;

namespace Branch.Packages.Contracts.ServiceHalo4
{
	public interface IService
	{
		Task<ResGetServiceRecord> GetServiceRecord(ReqGetServiceRecord req);
		
		Task<ResGetRecentMatches> GetRecentMatches(ReqGetRecentMatches req);
	}
}
