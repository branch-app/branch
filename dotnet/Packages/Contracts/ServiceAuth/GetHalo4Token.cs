using System;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceAuth
{
	public class ReqGetHalo4Token
	{
		public bool ForceRefresh { get; set; }
	}

	public class ResGetHalo4Token
	{
		public string SpartanToken { get; set; }

		public string Gamertag { get; set; }

		public string AnalyticToken { get; set; }

		public DateTime ExpiresAt { get; set; }
	}
}
