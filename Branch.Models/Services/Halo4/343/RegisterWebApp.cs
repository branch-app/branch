using System.Collections.Generic;

namespace Branch.Models.Services.Halo4._343
{
	public class RegisterWebApp
	{
		public string ClientStatusMessage { get; set; }
		public string Identifier { get; set; }
		public Enums.ResponseCode ResponseCode { get; set; }
		public Dictionary<string, string> ServiceList { get; set; }
		public Dictionary<string, string> Settings { get; set; } 
		public int State { get; set; }
		public int TokenResponseCode { get; set; }
	}
}
