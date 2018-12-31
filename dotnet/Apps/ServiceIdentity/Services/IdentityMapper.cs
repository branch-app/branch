using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Apps.ServiceIdentity.Models;
using Branch.Clients.XboxLive;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Extensions;

namespace Branch.Apps.ServiceIdentity.Services
{
	public class IdentityMapper
	{
		private XboxLiveClient xblClient { get; }
		private Dictionary<string, XboxLiveIdentity> gamertagMap { get; }
		private Dictionary<long, XboxLiveIdentity> xuidMap { get; }
		private Dictionary<string, Task<XboxLiveIdentity>> inProgressLookups { get; }
		private TimeSpan cacheExpiry = TimeSpan.FromMinutes(15);

		public IdentityMapper(XboxLiveClient xblClient)
		{
			this.xblClient = xblClient;

			this.gamertagMap = new Dictionary<string, XboxLiveIdentity>();
			this.xuidMap = new Dictionary<long, XboxLiveIdentity>();
			this.inProgressLookups = new Dictionary<string, Task<XboxLiveIdentity>>();
		}

		public async Task<XboxLiveIdentity> GetIdentity(XboxLiveIdentityType type, string value)
		{
			var key = $"{type.ToString().ToLower()}-{value.ToSlug()}";
			Task<XboxLiveIdentity> task = null;
			lock (inProgressLookups)
			{
				inProgressLookups.TryGetValue(key, out task);

				// There should never be a task in it's completed state in the dictionary,
				// but if there is it won't be good - so we double check.
				if (task == null || task.IsCompleted)
					inProgressLookups.TryAdd(key, (task = getIdentity(type, value)));
			}

			return await task;
		}

		private async Task<XboxLiveIdentity> getIdentity(XboxLiveIdentityType type, string value)
		{
			var sanitizedInput = value.ToSlug();
			var now = DateTime.UtcNow;
			XboxLiveIdentity identity = null;

			if (type == XboxLiveIdentityType.Gamertag)
				gamertagMap.TryGetValue(sanitizedInput, out identity);
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

			gamertagMap[identity.Gamertag.ToSlug()] = identity;
			xuidMap[identity.XUID] = identity;

			// Remove from pending list
			inProgressLookups.Remove(sanitizedInput);

			return identity;
		}
	}
}
