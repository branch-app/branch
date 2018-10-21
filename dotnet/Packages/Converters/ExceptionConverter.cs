using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Branch.Packages.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Branch.Packages.Converters
{
	// TODO(0xdeafcafe): Rename to BranchExceptionConverter
	public class ExceptionConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var exception = value as BranchException;
			var error = parseException(exception);
			var token = JToken.FromObject(error, serializer);
			var obj = token as JObject;

			obj.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var error = serializer.Deserialize(reader, typeof(ErrorBase)) as ErrorBase;

			return parseError(error);
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(BranchException);
		}

		private ErrorBase parseException(BranchException ex)
		{
			var error = new ErrorBase
			{
				Code = ex.Message,
				Meta = parseExceptionData(ex.Data),
				Reasons = ex.InnerExceptions.Select(inner => parseException(inner as BranchException)),
			};

			// Hacky workaround for aggregate exceptions just tacking messages together
			var spaceIndex = error.Code.IndexOf(' ');
			if (spaceIndex > 0)
				error.Code = error.Code.Remove(spaceIndex);

			if (!error.Reasons.Any()) error.Reasons = null;
			if (!error.Meta.Any()) error.Meta = null;

			return error;
		}

		private BranchException parseError(ErrorBase err)
		{
			var reasons = err.Reasons.Select(r => parseError(r));
			var exception = new BranchException(err.Code, err.Meta, reasons)
			{
				Reported = true
			};

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
