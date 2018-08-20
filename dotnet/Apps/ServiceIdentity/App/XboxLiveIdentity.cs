using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ksuid;
using Branch.Packages.Models.ServiceIdentity;

namespace Branch.Apps.ServiceIdentity.App
{
	public static partial class Application
	{
		public static async Task<XboxLiveIdentityResponse> XboxLiveIdentity(XboxLiveIdentityRequest req)
		{
			return new XboxLiveIdentityResponse { };
		}
	}
}
