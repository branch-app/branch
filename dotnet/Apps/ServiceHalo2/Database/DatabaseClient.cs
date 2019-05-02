using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Branch.Apps.ServiceHalo2.Models;
using Branch.Clients.Postgres;
using Branch.Packages.Bae;
using Branch.Packages.Extensions;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Branch.Apps.ServiceHalo2.Database
{
	public class DatabaseClient : PostgresClient
	{
		// TODO(0xdeafcafe): Maybe do some initial reflection to create some helper
		// methods for simplying Getting/Setting?

		public DatabaseClient(IOptions<PostgresConfig> config) : base(config) { }

		public async Task<CacheMeta> GetCacheMeta(string identifier)
		{
			using (var command = Connection.CreateCommand())
			{
				command.CommandText = "SELECT * FROM cache_meta WHERE identifier=@identifier LIMIT 1;";
				command.Parameters.AddWithValue("@identifier", identifier);

				using (var reader = await command.ExecuteReaderAsync())
				{
					if (!reader.HasRows)
						return null;

					await reader.ReadAsync();

					return new CacheMeta(reader);
				}
			}
		}

		public async Task SetCacheMeta(string identifier, string cacheState, BaeException cacheFailure = null)
		{
			using (var command = Connection.CreateCommand())
			{
				command.CommandText = @"
					INSERT INTO cache_meta
						(id, identifier, cache_state, cache_failure)
					VALUES
						(@id, @identifier, @cache_state, @cache_failure)
					ON CONFLICT (identifier)
					DO UPDATE SET
						cache_state = @cache_state, cache_failure = @cache_failure, updated_at = NOW();
				";

				command.Parameters.AddWithValue("@id", Ksuid.Ksuid.Generate("h2cachemeta").ToString());
				command.Parameters.AddWithValue("@identifier", identifier);
				command.Parameters.AddWithValueAsEnum("@cache_state", cacheState, "cache_status");
				command.Parameters.AddWithJsonOrNull("@cache_failure", cacheFailure);

				if (await command.ExecuteNonQueryAsync() != 1)
					throw new InvalidOperationException("Zero rows affected. Expected 1");
			}
		}

		public async Task<ServiceRecord> GetServiceRecord(string gamertag)
		{
			var escapedGt = gamertag.ToSlug();

			using (var command = Connection.CreateCommand())
			{
				command.CommandText = "SELECT * FROM service_records WHERE gamertag_ident=@gamertag_ident LIMIT 1;";
				command.Parameters.AddWithValue("@gamertag_ident", escapedGt);

				using (var reader = await command.ExecuteReaderAsync())
				{
					if (!reader.HasRows)
						return null;

					await reader.ReadAsync();

					return new ServiceRecord(reader);
				}
			}
		}

		public async Task SetServiceRecord(ServiceRecord sr)
		{
			sr.GamertagIdent = sr.Gamertag.ToSlug();

			using (var command = Connection.CreateCommand())
			{
				command.CommandText = @"
					INSERT INTO service_records
						(id, gamertag_ident, gamertag, emblem, clan_name, total_games, total_kills, total_deaths, total_assists, last_played)
					VALUES
						(@id, @gamertag_ident, @gamertag, @emblem, @clan_name, @total_games, @total_kills, @total_deaths, @total_assists, @last_played)
					ON CONFLICT (gamertag)
					DO UPDATE SET
						gamertag_ident = @gamertag_ident,
						gamertag = @gamertag,
						emblem = @emblem,
						clan_name = @clan_name,
						total_games = @total_games,
						total_kills = @total_kills,
						total_deaths = @total_deaths,
						total_assists = @total_assists,
						last_played = @last_played,
						updated_at = NOW();
				";

				command.Parameters.AddWithValue("@id", Ksuid.Ksuid.Generate("h2svcrec").ToString());
				command.Parameters.AddWithValue("@gamertag_ident", sr.GamertagIdent);
				command.Parameters.AddWithValue("@gamertag", sr.Gamertag);
				command.Parameters.AddWithValue("@emblem", sr.EmblemUrl);
				command.Parameters.AddWithValue("@clan_name", sr.ClanName);
				command.Parameters.AddWithValue("@total_games", sr.TotalGames);
				command.Parameters.AddWithValue("@total_kills", sr.TotalKills);
				command.Parameters.AddWithValue("@total_deaths", sr.TotalDeaths);
				command.Parameters.AddWithValue("@total_assists", sr.TotalAssists);
				command.Parameters.AddWithValue("@last_played", sr.LastPlayed);

				if (await command.ExecuteNonQueryAsync() != 1)
					throw new InvalidOperationException("Zero rows affected. Expected 1");
			}
		}
	}

	public abstract class Table
	{
		public Table() { }

		public Table(DbDataReader reader)
		{
			Id = reader.GetString(reader.GetOrdinal("id"));
			CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"));
			UpdatedAt = reader.GetDateTimeOrNull(reader.GetOrdinal("updated_at"));
		}

		public string Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public Nullable<DateTime> UpdatedAt { get; set; }
	}

	public class CacheMeta : Table
	{
		public CacheMeta() { }
		public CacheMeta(DbDataReader reader) : base(reader)
		{
			Identifer = reader.GetString(reader.GetOrdinal("identifier"));
			CacheState = reader.GetString(reader.GetOrdinal("cache_state"));
			CacheFailure = reader.GetJsonOrNull<BaeException>(reader.GetOrdinal("cache_failure"));
		}

		public string Identifer { get; set; }

		public string CacheState { get; set; }

		public BaeException CacheFailure { get; set; }
	}

	public class GamertagReplacement : Table
	{
		public GamertagReplacement() { }
		public GamertagReplacement(DbDataReader reader) : base(reader)
		{
			SourceGamertag = reader.GetString(reader.GetOrdinal("source_gamertag"));
			DestinationXuid = reader.GetInt64(reader.GetOrdinal("destination_xuid"));
		}


		public string SourceGamertag { get; set; }

		public long DestinationXuid { get; set; }
	}

	public class ServiceRecord : Table
	{
		public ServiceRecord() { }
		public ServiceRecord(DbDataReader reader) : base(reader)
		{
			Gamertag = reader.GetString(reader.GetOrdinal("gamertag"));
			EmblemUrl = reader.GetString(reader.GetOrdinal("emblem_url"));
			ClanName = reader.GetStringOrNull(reader.GetOrdinal("clan_name"));
			TotalGames = reader.GetInt32(reader.GetOrdinal("total_games"));
			TotalKills = reader.GetInt32(reader.GetOrdinal("total_kills"));
			TotalDeaths = reader.GetInt32(reader.GetOrdinal("total_deaths"));
			TotalAssists = reader.GetInt32(reader.GetOrdinal("total_assists"));
			LastPlayed = reader.GetDateTime(reader.GetOrdinal("last_played"));
		}

		public string GamertagIdent { get; set; }

		public string Gamertag { get; set; }

		public string EmblemUrl { get; set; }

		public string ClanName { get; set; }

		public int TotalGames { get; set; }

		public int TotalKills { get; set; }

		public int TotalDeaths { get; set; }

		public int TotalAssists { get; set; }

		public DateTime LastPlayed { get; set; }
	}
}
