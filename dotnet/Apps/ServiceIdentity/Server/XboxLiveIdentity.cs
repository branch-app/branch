using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ksuid;
using Branch.Packages.Contracts.ServiceIdentity;

namespace Branch.Apps.ServiceIdentity.Server
{
	public partial class RPC : IService
	{
		public async Task<ResGetXboxLiveIdentity> GetXboxLiveIdentity(ReqGetXboxLiveIdentity req)
		{
			return await app.GetXboxLiveIdentity(req.Type, req.Value);
		}

		public readonly string GetXboxLiveIdentitySchema = @"
			{
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
		";
	}
}
