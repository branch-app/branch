using System;
using Branch.Packages.Enums.ServiceIdentity;
using Newtonsoft.Json;

namespace Branch.Packages.Models.ServiceIdentity
{
	public class XboxLiveIdentityRequest
	{
		[JsonProperty("identity-type")]
		public XboxLiveIdentityType Type { get; set; }

		public string Value { get; set; }
	}

	public class XboxLiveIdentityResponse
	{
		// public 
	}
}
