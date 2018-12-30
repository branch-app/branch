using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceHalo4;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceIdentity;
using AutoMapper;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Models.Halo4;

namespace Branch.Apps.ServiceHalo4.App
{
	public partial class Application
	{
		public async Task<ResGetPlayerOverview> GetPlayerOverview(IdentityRequest identReq)
		{
			var identity = await identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = identReq.Type,
				Value = identReq.Value,
			});

			var (resp, cacheInfo) = await waypointClient.GetServiceRecord(identity.ToIdentity());
			var result = Mapper.Map<ResGetPlayerOverview>(resp);
			result.CacheInfo = cacheInfo;

			return result;
		}
	}
}
