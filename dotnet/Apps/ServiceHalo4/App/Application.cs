using System;
using System.Threading.Tasks;
using Branch.Apps.ServiceHalo4.Services;
using Branch.Clients.Auth;
using Branch.Clients.Identity;

namespace Branch.Apps.ServiceHalo4.App
{
	public partial class Application
	{
		private AuthClient authClient { get; }

		private IdentityClient identityClient { get; }

		public WaypointClient waypointClient { get; }

		public Application(AuthClient authClient, IdentityClient identityClient, WaypointClient waypointClient)
		{
			this.authClient = authClient;
			this.identityClient = identityClient;
			this.waypointClient = waypointClient;
		}
	}
}
