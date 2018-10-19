using Branch.Packages.Enums.ServiceIdentity;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.Common.Branch
{
	public class IdentityRequest
	{
		[JsonProperty("type")]
		public XboxLiveIdentityType Type { get; set; }


		[JsonProperty("value")]
		public string Value { get; set; }
	}
}
