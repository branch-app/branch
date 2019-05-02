using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Branch.Clients.Token;
using Branch.Apps.ServiceIdentity.Services;

namespace Branch.Apps.ServiceIdentity.App
{
	public partial class Application
	{
		private TokenClient tokenClient { get; }

		private IdentityMapper identityMapper { get; }

		public Application(TokenClient tokenClient, IdentityMapper identityMapper)
		{
			this.tokenClient = tokenClient;
			this.identityMapper = identityMapper;
		}
	}
}
