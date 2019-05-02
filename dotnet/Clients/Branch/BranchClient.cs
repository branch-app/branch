using System;
using System.Threading.Tasks;
using Branch.Clients.Json;
using Branch.Packages.Bae;
using Microsoft.Extensions.Options;

using HttpOptions = Branch.Clients.Http.Models.Options;

namespace Branch.Clients.Branch
{
	public class BranchClient
	{
		internal JsonClient Client { get; set; }

		public BranchClient(IOptionsMonitor<BranchConfig> options, string name)
		{
			var opts = options.Get(name);
			var httpOptions = new HttpOptions();

			httpOptions.Headers.Add("Authorization", $"bearer {opts.Key}");

			Client = new JsonClient(opts.Url, httpOptions);
		}

		public BranchClient(BranchConfig config)
		{
			var httpOptions = new HttpOptions();

			httpOptions.Headers.Add("Authorization", $"bearer {config.Key}");

			Client = new JsonClient(config.Url, httpOptions);
		}

		public async Task<TRes> Do<TRes>(string path, HttpOptions options = null)
			where TRes : class
		{
			return await Do<object, TRes>(path, null, options);
		}

		public async Task<TRes> Do<TReq, TRes>(string path, TReq body, HttpOptions options = null)
			where TReq : class
			where TRes : class
		{
			return await Client.Do<TReq, TRes, BaeException>("POST", path, null, body, options);
		}
	}
}
