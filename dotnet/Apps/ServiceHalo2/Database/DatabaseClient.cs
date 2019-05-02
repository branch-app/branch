using System;
using System.Threading.Tasks;
using Branch.Clients.Postgres;
using Branch.Packages.Extensions;
using Microsoft.Extensions.Options;

namespace Branch.Apps.ServiceHalo2.Database
{
	public class DatabaseClient : PostgresClient
	{
		public DatabaseClient(IOptions<PostgresConfig> config) : base(config) { }

		public async Task<ServiceRecord> GetServiceRecord(string gamertag)
		{
			var escapedGt = gamertag.ToSlug();

			using (var command = Connection.CreateCommand())
			{
				command.CommandText = "SELECT * FROM service_records WHERE gamertag = gamertag LIMIT 1;";
				command.Parameters.AddWithValue("@gamertag", escapedGt);

				using (var reader = await command.ExecuteReaderAsync())
				{
					if (!reader.HasRows)
						return null;

					await reader.ReadAsync();

					return new ServiceRecord
					{
						Id = reader.GetString(reader.GetOrdinal("id")),
						Gamertag = reader.GetString(reader.GetOrdinal("gamertag")),
						CacheState = reader.GetString(reader.GetOrdinal("cache_state")),
						CacheFailure = reader.GetValue(reader.GetOrdinal("cache_failure")),
						CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
						UpdatedAt = reader.GetDateTimeOrNull(reader.GetOrdinal("updated_at")),
					};
				}
			}
		}
	}

	public class ServiceRecord
	{
		public string Id { get; set; }

		public string Gamertag { get; set; }

		public string CacheState { get; set; }

		public object CacheFailure { get; set; }

		public DateTime CreatedAt { get; set; }

		public Nullable<DateTime> UpdatedAt { get; set; }
	}
}
