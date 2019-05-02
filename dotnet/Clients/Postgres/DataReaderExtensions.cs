using System;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

		public static string GetStringOrNull(this DbDataReader reader, int ordinal)
		{
			if (reader.IsDBNull(ordinal))
				return null;

			return reader.GetString(ordinal);
		}

		public static T GetJsonOrNull<T>(this DbDataReader reader, int ordinal)
			where T : class
		{
			var str = reader.GetStringOrNull(ordinal);

			if (str == null)
				return null;

			return JsonConvert.DeserializeObject<T>(str);
		}
	}
}
