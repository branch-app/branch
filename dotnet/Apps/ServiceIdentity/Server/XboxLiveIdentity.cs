using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ksuid;
using Branch.Packages.Models.ServiceIdentity;

namespace Branch.Apps.ServiceIdentity.Server
{
	public static partial class RPC
	{
		public static async Task<XboxLiveIdentityResponse> XboxLiveIdentity(XboxLiveIdentityRequest req)
		{
			return new XboxLiveIdentityResponse { };
		}

		public static readonly string XboxLiveIdentitySchema = @"
			{
				""type"": ""array"",
				""items"": {
					""type"": ""object"",
					""additionalProperties"": false,

					""required"": [
						""type"",
						""value""
					],

					""properties"": {
						""type"": {
							""type"": ""string"",
							""enum"": [""xuid"", ""gamertag""]
						},

						""value"": {
							""type"": ""string"",
							""minLength"": 1
						},
					}
				}
			}
		";
	}
}
