using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Packages.Exceptions
{
	public class ErrorBase
	{
		public string Code { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, object> Meta { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<ErrorBase> Reasons { get; set; } = null;
	}
}
