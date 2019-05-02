using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Branch.Apps.ServiceHalo2.Helpers;
using Branch.Apps.ServiceHalo2.Models;
using Branch.Packages.Bae;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceHalo2;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Extensions;
using Microsoft.Extensions.Logging;

namespace Branch.Apps.ServiceHalo2.App
{
	public partial class Application
	{
		public async Task<ResGetServiceRecord> GetServiceRecord(ReqGetServiceRecord req)
		{
			var escapedGt = req.Gamertag.ToSlug();
			var needsQueueing = await CacheMetaHelpers.NeedsQueueing(_databaseClient, $"sr-{escapedGt}");

			if (needsQueueing)
			{
				await _databaseClient.SetCacheMeta($"sr-{escapedGt}", "in_progress");

				TaskExt.FireAndForget(() => _sqsClient.SendMessageAsync(new QueueEvent
				{
					Type = QueueEventTypes.CacheServiceRecord,
					Payload = new ServiceRecordPayload { Gamertag = req.Gamertag },
				}));

				throw new BaeException("queued_for_caching");
			}

			// Fetch!
			var sr = await _databaseClient.GetServiceRecord(escapedGt);
			var response = Mapper.Map<ResGetServiceRecord>(sr);
			response.CacheInfo = new CacheInfo(sr.CreatedAt);

			return response;
		}
	}
}
