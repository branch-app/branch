using System;
using Branch.Core.Game.HaloReach.Enums;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.JsonConverters
{
	public class FileTypeConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof (string);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			FileType fileType;
			Enum.TryParse(reader.Value.ToString(), true, out fileType);
			return fileType;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue((value as FileType?).ToString());
		}
	}
}
