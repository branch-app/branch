using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceHalo4;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceIdentity;
using AutoMapper;

namespace Branch.Apps.ServiceHalo4.App
{
	public partial class Application
	{
		public async Task<ResGetServiceRecord> GetServiceRecord(IdentityRequest identityReq)
		{
			var identity = await identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = identityReq.Type,
				Value = identityReq.Value,
			});

			var response = await waypointClient.GetServiceRecord(identity.Gamertag);
			var result = Mapper.Map<ResGetServiceRecord>(response.serviceRecord);
			result.CacheInfo = response.cacheInfo;

			return result;
		}
	}
}
