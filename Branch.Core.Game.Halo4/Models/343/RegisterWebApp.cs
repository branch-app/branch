using System.Collections.Generic;
using Branch.Core.Game.Halo4.Enums;

namespace Branch.Core.Game.Halo4.Models._343
{
	public class RegisterWebApp
	{
		public string ClientStatusMessage { get; set; }
		public string Identifier { get; set; }
		public ResponseCode ResponseCode { get; set; }
		public Dictionary<string, string> ServiceList { get; set; }
		public Dictionary<string, string> Settings { get; set; }
		public int State { get; set; }
		public int TokenResponseCode { get; set; }
	}
}