using System.Threading.Tasks;

namespace Branch.Packages.Contracts.ServiceHalo2
{
	public interface IService
	{
		Task<ResGetServiceRecord> GetServiceRecord(ReqGetServiceRecord req);
	}
}
