using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Apollo.Models;
using Branch.Packages.Models.Common.Config;
using Branch.Packages.Enums.External.XboxLive;

namespace Branch.Apps.ServiceIdentity.Models
{
	public class XboxLiveError
	{
		public ResponseCode Code { get; set; }

		public string Source { get; set; }

		public string Description { get; set; }
	}
}
