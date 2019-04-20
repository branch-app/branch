using XboxLiveClientBase = Branch.Clients.XboxLive.XboxLiveClient;

namespace Branch.Apps.ServiceIdentity.Services
{
	public class XboxLiveClient : XboxLiveClientBase
	{
		public XboxLiveClient(TokenClient tokenClient)
			: base(tokenClient, null, null) { }

		/// <summary>
		/// Gets the profile settings of a player. If no settings are given, it is assumed
		/// all are wanted.
		/// </summary>
		public async Task<ProfileSettings> GetProfileSettings(XboxLiveIdentityType type, string value)
		{
			var path = string.Format(profileSettingsUrl, type.ToString(), value);
			var query = new Dictionary<string, string> { { "settings", string.Join(",", strs) } };

			return await requestXboxLiveData<ProfileSettings>(profileClient, 2, path, query, null);
		}
	}
}
