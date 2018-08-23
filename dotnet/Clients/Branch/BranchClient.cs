using System;
using System.Threading.Tasks;
using Branch.Clients.Json;
using Branch.Clients.Json.Models;
using Branch.Packages.Exceptions;

namespace Branch.Clients.Branch
{
	public class BranchClient
	{
		internal JsonClient Client { get; set; }

		public BranchClient(string baseUrl, string key, Options options = null)
		{
			options = options ?? new Options();
			options.Headers.Add("Authorization", $"bearer 01.{key}");

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
			return await Client.Do<TReq, TRes, BranchException>("POST", path, null, body, options);
		}
	}
}
