using Branch.Packages.Enums.ServiceIdentity;

namespace Branch.Packages.Contracts.Common.Branch
{
	public class IdentityRequest
	{
		public XboxLiveIdentityType Type { get; set; }

		public string Value { get; set; }
	}
}
