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
using Branch.Packages.Models.Halo2;
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

			// TODO(0xdeafcafe): Clean this shit up 🤮
			var sr = await _databaseClient.GetServiceRecord(escapedGt);
			var p1 = Mapper.Map<ServiceRecordResponse>(sr);
			var p2 = Mapper.Map<ResGetServiceRecord>(p1);
			p2.CacheInfo = new CacheInfo(sr.CreatedAt);

			return p2;
		}
	}
}
