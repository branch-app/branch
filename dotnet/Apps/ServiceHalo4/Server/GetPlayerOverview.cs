using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ksuid;
using Branch.Packages.Contracts.ServiceHalo4;

namespace Branch.Apps.ServiceHalo4.Server
{
	public partial class RPC : IService
	{
		public async Task<ResGetPlayerOverview> GetPlayerOverview(ReqGetPlayerOverview req)
		{
			return await app.GetPlayerOverview(req.Identity);
		}

		public readonly string GetPlayerOverviewSchema = @"
			{
				""type"": ""object"",
				""additionalProperties"": false,

				""required"": [
					""identity""
				],

				""properties"": {
					""identity"": {
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
							}
						}
					}
				}
			}
		";
	}
}
