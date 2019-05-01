using System;
using System.Threading.Tasks;
using Branch.Clients.Http.Models;
using Branch.Clients.Json;
using Branch.Packages.Bae;

namespace Branch.Clients.Branch
{
	public class BranchClient
	{
		internal JsonClient Client { get; set; }

		public BranchClient(string baseUrl, string key, Options options = null)
		{
			options = options ?? new Options();
			options.Headers.Add("Authorization", $"bearer {key}");

			Client = new JsonClient(baseUrl, options);
		}

		public async Task<TRes> Do<TRes>(string path, Options options = null)
			where TRes : class
		{
			return await Do<object, TRes>(path, null, options);
		}

		public async Task<TRes> Do<TReq, TRes>(string path, TReq body, Options options = null)
			where TReq : class
			where TRes : class
		{
			return await Client.Do<TReq, TRes, BaeException>("POST", path, null, body, options);
		}
	}
}
