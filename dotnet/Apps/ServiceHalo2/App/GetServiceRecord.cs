using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Apps.ServiceHalo2.Models;
using Branch.Packages.Bae;
using Branch.Packages.Contracts.ServiceHalo2;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Enums.ServiceIdentity;
using Microsoft.Extensions.Logging;

namespace Branch.Apps.ServiceHalo2.App
{
	public partial class Application
	{
		public async Task<ResGetServiceRecord> GetServiceRecord(ReqGetServiceRecord req)
		{
			var identity = await _identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = req.Identity.Type,
				Value = req.Identity.Value,
			});

			var srStatus = await _databaseClient.GetServiceRecord(identity.Gamertag);

			switch(srStatus?.CacheState)
			{
				case null:
					await _databaseClient.SetServiceRecord(identity.Gamertag, "in_progress");
					await _sqsClient.SendMessageAsync(new QueueEvent
					{
						Type = QueueEventTypes.CacheServiceRecord,
						Payload = new ServiceRecordPayload{ Gamertag = identity.Gamertag },
					});

					throw new BaeException("queued_for_caching");

				case "failed":
					throw srStatus.CacheFailure;

				case "in_progress":
					throw new BaeException("currently_caching");

				case "complete":
					throw new NotImplementedException("Fetch this from S3!");

				default:
					throw new NotSupportedException("Yuuuge failure!");
			}
		}
	}
}
