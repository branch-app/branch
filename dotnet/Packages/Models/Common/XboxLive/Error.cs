using Branch.Packages.Models.Common.Config;
using Branch.Packages.Enums.External.XboxLive;

namespace Branch.Models.Common.XboxLive
{
	public class Error
	{
		public ResponseCode Code { get; set; }

		public string Source { get; set; }

		public string Description { get; set; }
	}
}
