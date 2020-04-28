using System;
using System.Threading.Tasks;
using System.Web;
using Branch.Global.Models.Domain;
using Branch.Global.Extensions;
using Branch.Global.Libraries;
using Crpc.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace Branch.Services.Token.App
{
	public partial class Application
	{
		private readonly string XblRedisKey = "token:xbox-live";
		private readonly string XblAuthUrl = "https://login.live.com/oauth20_authorize.srf?client_id=0000000048093EE3&redirect_uri=https://login.live.com/oauth20_desktop.srf&response_type=token&display=touch&scope=service::user.auth.xboxlive.com::MBI_SSL";

		public async Task<GetXblTokenResponse> GetXblToken(HttpContext ctx, bool ignoreCache)
		{
			var useCache = !ignoreCache; // Easier to read non-negated conditions
			var xblProvider = _config.AuthProviders.XboxLive;

			using (var client = _redisClientsManager.GetClient())
			{
				if (useCache)
				{
					var token = client.GetJson<GetXblTokenResponse>(XblRedisKey);

					if (token != null && token.CacheInfo.IsFresh())
						return token;
				}

				using (var page = await _chromieTalkie.Browser.NewPageAsync())
				{
					await page.GoToAsync(XblAuthUrl);

					var hash = await page.EvaluateFunctionAsync<string>("() => window.location.hash");

					if (String.IsNullOrWhiteSpace(hash))
					{
						await page.TypeAndSubmitAsync(xblProvider.EmailAddress);
						await page.TypeAndSubmitAsync(xblProvider.Password);

						// Check to see if we need to accept the new terms.. Which we deffo are reading
						var title = await page.EvaluateFunctionAsync<string>(@"() => {
							const t = window.document.querySelector('#iPageTitle');

							return t ? t.textContent : null;
						}");

						if (!String.IsNullOrWhiteSpace(title) && title.Contains("terms"))
						{
							await Task.WhenAll(
								page.WaitForNavigationAsync(new NavigationOptions
								{
									WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle2 }
								}),
								page.ClickAsync("input[type=submit]")
							);
						}
					}

					hash = await page.EvaluateFunctionAsync<string>("() => window.location.hash");

					if (String.IsNullOrWhiteSpace(hash.ToString()))
						throw new CrpcException("token_read_failed");

					var parsedHash = HttpUtility.ParseQueryString(hash.ToString().Substring(1));
					var accessToken = parsedHash.Get("access_token");
					var token = await XboxLiveAuthClient.ExchangeOAuthToken(accessToken);
					var response = new GetXblTokenResponse
					{
						CacheInfo = new CacheInfo(token.IssueInstant, token.NotAfter),
						Token = token.Token,
						Uhs = token.DisplayClaims.Xui[0].Uhs,
					};

					client.Set(XblRedisKey, JsonConvert.SerializeObject(response), (DateTime) response.CacheInfo.ExpiresAt);

					return response;
				}
			}
		}
	}
}
