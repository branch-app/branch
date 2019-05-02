using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Branch.LambdaFunctions.TokenRefresh.Models;

using Amazon.Lambda.Core;
using Branch.Clients.Token;
using Branch.Packages.Contracts.ServiceToken;

namespace Branch.LambdaFunctions.TokenRefresh
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
			var tokenConfig = config.Services["Token"];
			var tokenClient = new TokenClient(tokenConfig);

			var tasks = Task.WhenAll(new List<Task>
			{
				tokenClient.GetHalo4Token(new ReqGetHalo4Token { ForceRefresh = true }),
				tokenClient.GetXboxLiveToken(new ReqGetXboxLiveToken { ForceRefresh = true }),
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
