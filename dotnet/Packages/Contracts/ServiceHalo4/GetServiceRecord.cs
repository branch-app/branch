using System;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Models.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceHalo4
{
	public class ReqGetServiceRecord
	{
		public IdentityRequest Identity { get; set; }
	}

	public class ResGetServiceRecord : ServiceRecordResponse, IBranchResponse
	{
		public ICacheInfo CacheInfo { get; set; }
	}
}
