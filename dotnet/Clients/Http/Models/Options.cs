using System;
using System.Collections.Generic;

namespace Branch.Clients.Http.Models
{
	public class Options
	{
		public Options(Dictionary<string, string> headers = null, TimeSpan? timeout = null)
		{
			Headers = headers ?? new Dictionary<string, string>();
			Timeout = timeout ?? TimeSpan.FromMilliseconds(2000);
		}

		public Dictionary<string, string> Headers { get; set; }

		public TimeSpan Timeout { get; set; }
	}
}
