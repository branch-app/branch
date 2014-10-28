using Newtonsoft.Json;

namespace Branch.Models.Services.HostIp
{
	public class Response
	{
		[JsonProperty("country_name")]
		public string CountryName { get; set; }
		
		[JsonProperty("country_code")]
		public string CountryCode { get; set; }
		
		[JsonProperty("ip")]
		public string IpAddress { get; set; }
		
		[JsonProperty("lat")]
		public double Latitude { get; set; }
		
		[JsonProperty("lng")]
		public double Longitude { get; set; }
	}
}
