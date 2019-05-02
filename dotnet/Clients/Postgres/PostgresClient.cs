using System;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Branch.Clients.Postgres
{
	public class PostgresClient : IDisposable
	{
		protected NpgsqlConnection Connection { get; }

		public PostgresClient(IOptions<PostgresConfig> options)
		{
			// Setup Plugins
			NpgsqlConnection.GlobalTypeMapper.UseJsonNet();

			Connection = new NpgsqlConnection(options.Value.ConnectionString);
			Connection.Open();
		}

		public void Dispose()
		{
			Connection.Close();
		}
	}
}
