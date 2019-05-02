using Branch.Apps.ServiceHalo4.Services;
using Branch.Clients.Token;
using Branch.Clients.Identity;

namespace Branch.Apps.ServiceHalo4.App
{
	public partial class Application
	{
		public readonly TokenClient _tokenClient;

		public readonly IdentityClient _identityClient;

		public readonly WaypointClient _waypointClient;

		public Application(TokenClient tokenClient, IdentityClient identityClient, WaypointClient waypointClient)
		{
			this._tokenClient = tokenClient;
			this._identityClient = identityClient;
			this._waypointClient = waypointClient;
		}
	}
}
