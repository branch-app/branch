using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Branch.Packages.Bae
{
	public class BaeExceptionConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var exception = value as BaeException;
			var error = parseException(exception);
			var token = JToken.FromObject(error, serializer);
			var obj = token as JObject;

			obj.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var error = serializer.Deserialize(reader, typeof(BaeExceptionFormat)) as BaeExceptionFormat;

			return parseError(error);
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(BaeException);
		}

		private BaeExceptionFormat parseException(BaeException ex)
		{
			var error = new BaeExceptionFormat
			{
				Code = ex.Message,
				Meta = parseExceptionData(ex.Data),
				Reasons = ex.InnerExceptions.Select(inner => parseException(inner as BaeException)),
			};

			// Aggregate Exceptions concatenate the "message" of every exception together
			// with a space as a seperator.. This is the way to ensure we get the first
			// error message, and none of the rest.

			var spaceIndex = error.Code.IndexOf(' ');
			if (spaceIndex > 0)
				error.Code = error.Code.Remove(spaceIndex);

			if (!error.Reasons.Any()) error.Reasons = null;
			if (!error.Meta.Any()) error.Meta = null;

			return error;
		}

		private BaeException parseError(BaeExceptionFormat err)
		{
			var reasons = err.Reasons.Select(r => parseError(r));
			var exception = new BaeException(err.Code, err.Meta, reasons);

			return exception;
		}

		private Dictionary<string, object> parseExceptionData(IDictionary data)
		{
			return data
				.Cast<DictionaryEntry>()
				.ToDictionary(de => de.Key as string, de => de.Value as object);
		}
	}
}
