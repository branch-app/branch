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

			// TODO(0xdeafcafe): If lookup is an XUID reject if there are no "verified"
			// gamertags.

			var payload = new QueueEvent
			{
				Type = QueueEventTypes.CacheServiceRecord,
				Payload = new ServiceRecordPayload{ Gamertag = identity.Gamertag },
			};

			await _sqsClient.SendMessageAsync(payload);

			throw new BaeException("queued_for_caching");
		}
	}
}
