using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceHalo4;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceIdentity;
using AutoMapper;
using Branch.Packages.Enums.ServiceIdentity;

namespace Branch.Apps.ServiceHalo4.App
{
	public partial class Application
	{
		public async Task<ResGetServiceRecord> GetServiceRecord(IdentityRequest identReq)
		{
			var identity = await identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = identReq.Type,
				Value = identReq.Value,
			});

			var response = await waypointClient.GetServiceRecord(identity.ToIdentity());
			var result = Mapper.Map<ResGetServiceRecord>(response.serviceRecord);
			result.CacheInfo = response.cacheInfo;

			return result;
		}
	}
}
