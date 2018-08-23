using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ksuid;
using Branch.Clients.Auth;
using Branch.Apps.ServiceIdentity.Services;

namespace Branch.Apps.ServiceIdentity.App
{
	public partial class Application
	{
		private AuthClient authClient { get; }

		private IdentityMapper identityMapper { get; }

		public Application(AuthClient authClient, IdentityMapper identityMapper)
		{
			this.authClient = authClient;
			this.identityMapper = identityMapper;
		}
	}
}
