using System.Collections.Generic;
using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceHalo2;

namespace Branch.Apps.ServiceHalo2.Server
{
	public partial class RPC : IService
	{
		public async Task<ResGetServiceRecord> GetServiceRecord(ReqGetServiceRecord req)
		{
			return await _app.GetServiceRecord(req);
		}

		public static readonly string GetServiceRecordSchema = @"
			{
				""type"": ""object"",
				""additionalProperties"": false,

				""required"": [
					""gamertag""
				],

				""properties"": {
					""gamertag"": {
						""type"": ""string"",
						""minLength"": 1
					}
				}
			}
		";
	}
}
