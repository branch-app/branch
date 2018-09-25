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
		public async Task<ResGetRecentMatches> GetRecentMatches(ReqGetRecentMatches req)
		{
			var identity = req.Identity;
			var gameMode = req.GameMode;
			var startAt = req.StartAt ?? 0;
			var count = req.Count ?? 25;

			return await app.GetRecentMatches(identity, gameMode, startAt, count);
		}

		public readonly string GetRecentMatchesSchema = @"
			{
				""type"": ""object"",
				""additionalProperties"": false,

				""required"": [
					""identity"",
					""game_mode""
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
					},

					""game_mode"": {
						""type"": ""string"",
						""enum"": [""war-games"", ""campaign"", ""spartan-ops"", ""custom-games""]
					},

					""start_at"": {
						""type"": [""integer"", ""null""],
						""minimum"": 0
					},

					""count"": {
						""type"": [""integer"", ""null""],
						""multipleOf"": 1.0,
						""minimum"": 1,
						""maximum"": 50
					}
				}
			}
		";
	}
}
