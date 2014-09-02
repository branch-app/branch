using System;
using Branch.Core.Game.HaloReach.Enums;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.JsonConverters
{
	public class MapTypeConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof (string);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			switch (reader.Value.ToString())
			{
				case "cp":
					return MapType.Campaign;
				case "mm":
					return MapType.Multiplayer;
				case "ff":
					return MapType.Firefight;
			}

			return MapType.Mainmenu;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			switch (value as MapType?)
			{
				case MapType.Campaign:
					writer.WriteValue("cp");
					break;
				case MapType.Multiplayer:
					writer.WriteValue("mm");
					break;
				case MapType.Firefight:
					writer.WriteValue("ff");
					break;
				case MapType.Mainmenu:
					writer.WriteValue("mu");
					break;
			}
		}
	}
}
