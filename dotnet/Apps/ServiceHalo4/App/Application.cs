using System;
using System.Threading.Tasks;
using Branch.Apps.ServiceHalo4.Services;
using Branch.Clients.Token;
using Branch.Clients.Identity;

namespace Branch.Apps.ServiceHalo4.App
{
	public partial class Application
	{
		private TokenClient tokenClient { get; }

		private IdentityClient identityClient { get; }

		public WaypointClient waypointClient { get; }

		public Application(TokenClient tokenClient, IdentityClient identityClient, WaypointClient waypointClient)
		{
			this.tokenClient = tokenClient;
			this.identityClient = identityClient;
			this.waypointClient = waypointClient;
		}
	}
}
