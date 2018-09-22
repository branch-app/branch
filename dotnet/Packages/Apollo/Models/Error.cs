using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Packages.Apollo.Models
{
	public class Error
	{
		public string Code { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, object> Meta { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<Error> Reasons { get; set; } = null;
	}
}
