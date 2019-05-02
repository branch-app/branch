using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Apps.ServiceIdentity.Models;
using Branch.Clients.XboxLive;
using Branch.Packages.Enums.External.XboxLive;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Extensions;

namespace Branch.Apps.ServiceIdentity.Services
{
	public class IdentityMapper
	{
		private readonly XboxLiveClient _xblClient;
		private readonly Dictionary<string, XboxLiveIdentity> _gamertagMap;
		private readonly Dictionary<long, XboxLiveIdentity> _xuidMap;
		private readonly Dictionary<string, Task<XboxLiveIdentity>> _inProgressLookups;
		private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(15);

		public IdentityMapper(XboxLiveClient xblClient)
		{
			this._xblClient = xblClient;

			this._gamertagMap = new Dictionary<string, XboxLiveIdentity>();
			this._xuidMap = new Dictionary<long, XboxLiveIdentity>();
			this._inProgressLookups = new Dictionary<string, Task<XboxLiveIdentity>>();
		}

		public async Task<XboxLiveIdentity> GetIdentity(XboxLiveIdentityType type, string value)
		{
			var key = $"{type.ToString().ToLower()}-{value.ToSlug()}";
			Task<XboxLiveIdentity> task = null;
			lock (_inProgressLookups)
			{
				_inProgressLookups.TryGetValue(key, out task);

				// There should never be a task in it's completed state in the dictionary,
				// but if there is it won't be good - so we double check.
				if (task == null || task.IsCompleted)
					_inProgressLookups.TryAdd(key, (task = getIdentity(type, value)));
			}

			return await task;
		}

		private async Task<XboxLiveIdentity> getIdentity(XboxLiveIdentityType type, string value)
		{
			var sanitizedInput = value.ToSlug();
			var now = DateTime.UtcNow;
			XboxLiveIdentity identity = null;

			if (type == XboxLiveIdentityType.Gamertag)
				_gamertagMap.TryGetValue(sanitizedInput, out identity);
			else if (type == XboxLiveIdentityType.Xuid)
				_xuidMap.TryGetValue(long.Parse(value), out identity);

			if (identity != null && identity.ExpiresAt > now)
				return identity;

			var resp = await _xblClient.GetProfileSettings(type, value, ProfileSetting.Gamertag);
			var user = resp.ProfileUsers[0];

			identity = new XboxLiveIdentity
			{
				XUID = user.ID,
				Gamertag = user.Settings.First(s => s.ID == "Gamertag").Value,
				CachedAt = now,
				ExpiresAt = now.Add(_cacheExpiry),
			};

			_gamertagMap[identity.Gamertag.ToSlug()] = identity;
			_xuidMap[identity.XUID] = identity;

			// Remove from pending list
			_inProgressLookups.Remove(sanitizedInput);

			return identity;
		}
	}
}
