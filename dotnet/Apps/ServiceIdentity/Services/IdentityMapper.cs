using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Apps.ServiceIdentity.Models;
using Branch.Packages.Enums.ServiceIdentity;

namespace Branch.Apps.ServiceIdentity.Services
{
	public class IdentityMapper
	{
		private XboxLiveClient xblClient { get; }
		private Dictionary<string, XboxLiveIdentity> gamertagMap { get; } = new Dictionary<string, XboxLiveIdentity>();
		private Dictionary<long, XboxLiveIdentity> xuidMap { get; } = new Dictionary<long, XboxLiveIdentity>();
		private TimeSpan cacheExpiry = TimeSpan.FromMinutes(15);

		public IdentityMapper(XboxLiveClient xblClient)
		{
			this.xblClient = xblClient;
		}

		public async Task<XboxLiveIdentity> GetIdentity(XboxLiveIdentityType type, string value)
		{
			var now = DateTime.UtcNow;
			XboxLiveIdentity identity = null;

			if (type == XboxLiveIdentityType.Gamertag)
				gamertagMap.TryGetValue(sanitizeGamertag(value), out identity);
			else if (type == XboxLiveIdentityType.Xuid)
				xuidMap.TryGetValue(long.Parse(value), out identity);

			if (identity != null && identity.ExpiresAt > now)
				return identity;

			var resp = await xblClient.GetProfileSettings(type, value);
			var user = resp.ProfileUsers[0];

			identity = new XboxLiveIdentity
			{
				XUID = user.ID,
				Gamertag = user.Settings.First(s => s.ID == "Gamertag").Value,
				CachedAt = now,
				ExpiresAt = now.Add(cacheExpiry),
			};

			gamertagMap[sanitizeGamertag(identity.Gamertag)] = identity;
			xuidMap[identity.XUID] = identity;

			return identity;
		}

		private string sanitizeGamertag(string gamertag)
		{
			return gamertag.ToLower();
		}
	}
}
