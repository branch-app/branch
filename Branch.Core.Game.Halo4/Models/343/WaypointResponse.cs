using Branch.Core.Game.Halo4.Enums;
using Branch.Models;

namespace Branch.Core.Game.Halo4.Models._343
{
	public class Response 
		: BaseResponse
	{
		public ResponseCode StatusCode { get; set; }

		public string StatusResponse { get; set; }
	}
}
