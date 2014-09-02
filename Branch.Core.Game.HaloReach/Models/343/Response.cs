using Branch.Core.Game.HaloReach.Enums;
using Branch.Models;

namespace Branch.Core.Game.HaloReach.Models._343
{
	public class Response
		: BaseResponse
	{
		public string Reason { get; set; }

		public ResponseStatus Status { get; set; }
	}
}
