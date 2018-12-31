using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ksuid;
using Branch.Clients.Token;

namespace Branch.Apps.ServiceXboxLive.App
{
	public partial class Application
	{
		private TokenClient tokenClient { get; set; }

		public Application(TokenClient tokenClient)
		{
			this.tokenClient = tokenClient;
		}
	}
}
