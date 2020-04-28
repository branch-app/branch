using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Branch.Global.Libraries
{
	public static class XboxLiveAuthClient
	{
		private static JsonClient _userAuthClient { get; }
		private static JsonClient _xstsClient { get; }

		static XboxLiveAuthClient()
		{
			_userAuthClient = new JsonClient("https://user.auth.xboxlive.com");
			_xstsClient = new JsonClient("https://xsts.auth.xboxlive.com");
		}

		public async static Task<XboxLiveAuthToken> ExchangeOAuthToken(string token)
		{
			var userAuthRequest = new XblAuthRequest<UserAuthProperties>
			{
				Properties = new UserAuthProperties { RpsTicket = token },
				RelyingParty = "http://auth.xboxlive.com"
			};
			var userAuth = await _userAuthClient.Do<UserAuthResponse>("POST", "user/authenticate", userAuthRequest);

			var xstsAuthRequest = new XblAuthRequest<XstsAuthProperties>
			{
				Properties = new XstsAuthProperties { UserTokens = new string[] { userAuth.Token } },
				RelyingParty = "http://xboxlive.com"
			};
			var xstsAuth = await _xstsClient.Do<XboxLiveAuthToken>("POST", "xsts/authorize", xstsAuthRequest);

			return xstsAuth;
		}

		private class XblAuthRequest<T>
		{
			[JsonProperty("Properties")]
			public T Properties { get; set; }

			[JsonProperty("RelyingParty")]
			public string RelyingParty { get; set; }

			[JsonProperty("TokenType")]
			public string TokenType { get; set; } = "JWT";
		}

		private class UserAuthProperties
		{
			[JsonProperty("AuthMethod")]
			public string AuthMethod { get; set; } = "RPS";

			[JsonProperty("SiteName")]
			public string SiteName { get; set; } = "user.auth.xboxlive.com";

			[JsonProperty("RpsTicket")]
			public string RpsTicket { get; set; }
		}

		private class XstsAuthProperties
		{
			[JsonProperty("SandboxId")]
			public string SandboxId { get; set; } = "RETAIL";

			[JsonProperty("UserTokens")]
			public string[] UserTokens { get; set; }
		}

		private class UserAuthResponse
		{
			[JsonProperty("Token")]
			public string Token { get; set; }
		}
	}

	public class XboxLiveAuthToken
	{
		[JsonProperty("IssueInstant")]
		public DateTime IssueInstant { get; set; }

		[JsonProperty("NotAfter")]
		public DateTime NotAfter { get; set; }

		[JsonProperty("Token")]
		public string Token { get; set; }

		[JsonProperty("DisplayClaims")]
		public XboxLiveAuthDisplayClaims DisplayClaims { get; set; }
	}

	public class XboxLiveAuthDisplayClaims
	{
		[JsonProperty("xui")]
		public XboxLiveAuthDisplayClaim[] Xui { get; set; }
	}

	public class XboxLiveAuthDisplayClaim
	{
		[JsonProperty("uhs")]
		public string Uhs { get; set; }
	}
}
