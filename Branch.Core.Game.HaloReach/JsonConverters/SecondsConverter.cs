using System;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.JsonConverters
{
	public class SecondsConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(int);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return TimeSpan.FromSeconds(int.Parse(reader.Value.ToString()));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
