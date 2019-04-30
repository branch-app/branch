using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Packages.Bae
{
	internal class BaeExceptionFormat
	{
		public string Code { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, object> Meta { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<BaeExceptionFormat> Reasons { get; set; } = null;
	}
}
