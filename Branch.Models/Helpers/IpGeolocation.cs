using System;
using System.Net.Http;
using Branch.Models.Services.HostIp;
using Newtonsoft.Json;

namespace Branch.Models.Helpers
{
	public static class IpGeolocation
	{
		private const string ApiUrl = "http://api.hostip.info/get_json.php?ip={0}&position=true";

		public static Response Geolocate(string ipAddress)
		{
			try
			{
				return
					JsonConvert.DeserializeObject<Response>(
						((new HttpClient().GetAsync(String.Format(ApiUrl, ipAddress))).Result.Content.ReadAsStringAsync()).Result);
			}
			catch (Exception)
			{
				return new Response
				{
					Latitude = -1337,
					Longitude = -1337,
					CountryCode = "UNK",
					CountryName = "Unknown",
					IpAddress = ipAddress
				};
			}
		}
	}
}
