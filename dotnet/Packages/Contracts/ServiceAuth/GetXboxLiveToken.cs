using System;
using Newtonsoft.Json;

namespace Branch.Packages.Contracts.ServiceAuth
{
	public class ReqGetXboxLiveToken
	{
		public bool ForceRefresh { get; set; }
	}

	public class ResGetXboxLiveToken
	{
		public string Token { get; set; }

		public string Agg { get; set; }

		public string Gtg { get; set; }

		public string Prv { get; set; }

		public string Usr { get; set; }

		public string Utr { get; set; }

		public string Xid { get; set; }

		public string Uhs { get; set; }

		public DateTime ExpiresAt { get; set; }
	}
}
