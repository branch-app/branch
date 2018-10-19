using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Branch.LambdaFunctions.AuthRefresh.Models;

using Amazon.Lambda.Core;
using Branch.Clients.Auth;
using Branch.Packages.Contracts.ServiceAuth;

namespace Branch.LambdaFunctions.AuthRefresh
{
	public class Entrypoint
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public void Main(ILambdaContext context)
		{
			var config = parseConfig();
			var authConfig = config.Services["Auth"];
			var authClient = new AuthClient(authConfig.Url, authConfig.Key);

			var tasks = Task.WhenAll(new List<Task>
			{
				authClient.GetHalo4Token(new ReqGetHalo4Token { ForceRefresh = true }),
				authClient.GetXboxLiveToken(new ReqGetXboxLiveToken { ForceRefresh = true }),
			});

			tasks.Wait();

			if (tasks.Exception != null)
				throw tasks.Exception;
		}

		private Config parseConfig()
		{
			var str = Environment.GetEnvironmentVariable("CONFIG");

			return JsonConvert.DeserializeObject<Config>(str);
		}
	}
}
