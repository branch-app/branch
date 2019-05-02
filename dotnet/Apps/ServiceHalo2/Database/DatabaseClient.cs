using System;
using System.Threading.Tasks;
using Branch.Clients.Postgres;
using Branch.Packages.Bae;
using Branch.Packages.Extensions;
using Microsoft.Extensions.Options;
using Npgsql;

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
				command.CommandText = "SELECT * FROM service_records WHERE gamertag=@gamertag LIMIT 1;";
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
						CacheFailure = reader.GetJsonOrNull<BaeException>(reader.GetOrdinal("cache_failure")),
						CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
						UpdatedAt = reader.GetDateTimeOrNull(reader.GetOrdinal("updated_at")),
					};
				}
			}
		}

		public async Task SetServiceRecord(string gamertag, string cacheState, BaeException cacheFailure = null)
		{
			var escapedGt = gamertag.ToSlug();

			using (var command = Connection.CreateCommand())
			{
				command.CommandText = @"
					INSERT INTO service_records
						(id, gamertag, cache_state, cache_failure)
					VALUES
						(@id, @gamertag, @cache_state, @cache_failure)
					ON CONFLICT (gamertag)
					DO UPDATE SET
						cache_state = @cache_state, cache_failure = @cache_failure, updated_at = NOW();
				";

				command.Parameters.AddWithValue("@id", Ksuid.Ksuid.Generate("h2sr").ToString());
				command.Parameters.AddWithValue("@gamertag", escapedGt);
				command.Parameters.AddWithValueAsEnum("@cache_state", cacheState, "cache_status");
				command.Parameters.AddWithValueOrNull("@cache_failure", cacheFailure);

				if (await command.ExecuteNonQueryAsync() != 1)
					throw new InvalidOperationException("Zero rows affected. Expected 1");
			}
		}
	}

	public class ServiceRecord
	{
		public string Id { get; set; }

		public string Gamertag { get; set; }

		public string CacheState { get; set; }

		public BaeException CacheFailure { get; set; }

		public DateTime CreatedAt { get; set; }

		public Nullable<DateTime> UpdatedAt { get; set; }
	}
}
