using System;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceToken
{
	public class ReqGetXboxLiveToken
	{
		[JsonProperty("force_refresh")]
		public bool ForceRefresh { get; set; }
	}

	public class ResGetXboxLiveToken
	{
		[JsonProperty("token")]
		public string Token { get; set; }

		[JsonProperty("agg")]
		public string Agg { get; set; }

		[JsonProperty("gtg")]
		public string Gtg { get; set; }

		[JsonProperty("prv")]
		public string Prv { get; set; }

		[JsonProperty("usr")]
		public string Usr { get; set; }

		[JsonProperty("utr")]
		public string Utr { get; set; }

		[JsonProperty("xid")]
		public string Xid { get; set; }

		[JsonProperty("uhs")]
		public string Uhs { get; set; }

		[JsonProperty("expires_at")]
		public DateTime ExpiresAt { get; set; }
	}
}
