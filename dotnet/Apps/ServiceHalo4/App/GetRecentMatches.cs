using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceHalo4;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceIdentity;
using AutoMapper;
using Branch.Packages.Enums.ServiceIdentity;
using System.Linq;
using Branch.Packages.Enums.Halo4;

namespace Branch.Apps.ServiceHalo4.App
{
	public partial class Application
	{
		public async Task<ResGetRecentMatches> GetRecentMatches(IdentityRequest identReq, GameMode gameMode, uint startAt, uint count)
		{
			var identity = await identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = identReq.Type,
				Value = identReq.Value,
			});

			return null;

			// var paginationTestCount = count + 1;
			// var response = await waypointClient.GetRecentMatches(identity.ToIdentity(), gameMode, startAt, paginationTestCount);
			// var matches = response.recentMatches.Games;
			// var hasMoreMatches = matches.Length == paginationTestCount;

			// if (hasMoreMatches)
			// 	matches = matches.Take((int) count).ToArray();

			// return new ResGetRecentMatches
			// {
			// 	CacheInfo = response.cacheInfo,
			// 	Matches = matches,
			// 	DateFidelity = response.recentMatches.DateFidelity,
			// 	HasMoreMatches = hasMoreMatches,
			// };
		}
	}
}
