using System;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceAuth
{
	public class ReqGetHalo4Token
	{
		[JsonProperty("force_refresh")]
		public bool ForceRefresh { get; set; }
	}

	public class ResGetHalo4Token
	{
		[JsonProperty("spartan_token")]
		public string SpartanToken { get; set; }

		[JsonProperty("gamertag")]
		public string Gamertag { get; set; }

		[JsonProperty("analytic_token")]
		public string AnalyticToken { get; set; }

		[JsonProperty("expires_at")]
		public DateTime ExpiresAt { get; set; }
	}
}
