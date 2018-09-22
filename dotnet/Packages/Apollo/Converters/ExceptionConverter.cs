using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Branch.Packages.Apollo.Models;
using Branch.Packages.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apollo.Converters
{
	public class ExceptionConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			// TODO(0xdeafcafe): Handle non-branch

			var exception = value as BranchException;
			var error = parseException(exception);
			var token = JToken.FromObject(error, serializer);
			var obj = token as JObject;

			obj.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException("Not needed as we will never serialize this way.");
		}

		public override bool CanRead
		{
			get { return false; }
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Exception) || objectType.IsSubclassOf(typeof(Exception));
		}

		private Error parseException(BranchException ex)
		{
			var error = new Error
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

		private Dictionary<string, object> parseExceptionData(IDictionary data)
		{
			return data
				.Cast<DictionaryEntry>()
				.ToDictionary(de => de.Key as string, de => de.Value as object);
		}
	}
}
