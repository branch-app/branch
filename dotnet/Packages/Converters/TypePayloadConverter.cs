using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Packages.Converters
{
	public interface ITypePayload<T>
	{
		string Type { get; set; }

		T Payload { get; set; }
	}

	public class TypePayloadConverter<T, T1> : JsonConverter
		where T : ITypePayload<T1>, new()
		where T1 : class
	{
		private readonly Dictionary<string, Type> _mappings;

		public TypePayloadConverter(params object[] inputs)
		{
			// I was going to make this a LINQ query but thought that would be a little
			// too fucking rude.

			_mappings = new Dictionary<string, Type>();

			foreach (var input in inputs)
			{
				var fuckMe = input as object[];
				var code = fuckMe[0] as string;
				var type = fuckMe[1] as Type;

				_mappings.Add(code, type);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var @event = new T();

			do
			{
				if (checkStringValue(reader, "type"))
				{
					@event.Type = reader.ReadAsString();
					continue;
				}
				else if (checkStringValue(reader, "payload"))
				{
					@event.Payload = deserializePayload(@event.Type, reader, serializer);
					reader.Read();
					break;
				}
			} while (reader.Read());

			return @event;
		}

		private T1 deserializePayload(string type, JsonReader reader, JsonSerializer serializer)
		{
			// Step into Object
			reader.Read();

			if (!_mappings.ContainsKey(type))
				throw new InvalidOperationException("mapping doesnt contain required type");

			return serializer.Deserialize(reader, _mappings[type]) as T1;
		}

		public override bool CanConvert(Type objectType) => true;
		public override bool CanRead => true;
		public override bool CanWrite => false;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException("never called as never used");
		}

		private bool checkStringValue(JsonReader reader, string str)
		{
			if (reader.ValueType != typeof(string))
				return false;

			return reader.Value as string == str;
		}
	}
}
