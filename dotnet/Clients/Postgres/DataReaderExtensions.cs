using System;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Branch.Clients.Postgres
{
	public static class DataReaderExtensions
	{
		public static Nullable<DateTime> GetDateTimeOrNull(this DbDataReader reader, int ordinal)
		{
			if (reader.IsDBNull(ordinal))
				return null;

			return reader.GetDateTime(ordinal);
		}
	}
}
