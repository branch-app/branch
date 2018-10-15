using System;
using System.Xml;
using Branch.Packages.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Branch.Packages.Converters
{
	public class TimespanConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType == typeof(TimeSpan?);

		public override bool CanRead => true;

		public override bool CanWrite => true;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var spanString = reader.Value as string;

			if (spanString == null)
				return null;

			return XmlConvert.ToTimeSpan(spanString);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var duration = value as TimeSpan?;

			if (duration == null)
				writer.WriteNull();

			writer.WriteValue(XmlConvert.ToString((TimeSpan) duration));
		}
	}
}
