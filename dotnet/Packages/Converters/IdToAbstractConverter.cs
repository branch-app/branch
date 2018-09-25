using System;
using Branch.Packages.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Branch.Packages.Converters
{
	public class IdToAbstractConverter<T1, T2, T3, T4> : JsonConverter
		where T1 : class, new()
		where T2 : class, new()
		where T3 : class, new()
		where T4 : class, new()
	{
		private string keyName { get; }
		
		private string keyNameDangerNoodle { get; }

		private string[] values { get; }

		public IdToAbstractConverter(string keyName, params string[] values)
		{
			if (values.Length != 4)
				throw new IndexOutOfRangeException("length of values must be 3");

			this.keyName = keyName;
			this.keyNameDangerNoodle = keyName.ToSnakeCase();
			this.values = values;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var valType = value.GetType();

			if (!valType.IsAbstract)
			{
				serializer.ContractResolver.ResolveContract(valType).Converter = null;
				serializer.Serialize(writer, value);

				return;
			}

			serializer.Serialize(writer, value);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (!objectType.IsAbstract)
			{
				serializer.ContractResolver.ResolveContract(objectType).Converter = null;

				return serializer.Deserialize(reader, objectType);
			}

			var obj = JObject.Load(reader);
			var key = obj.SelectToken(keyName) ?? obj.SelectToken(keyNameDangerNoodle);
			var keyVal = key.ToString();

			if (keyVal == values[0])
				return obj.ToObject<T1>(serializer);
			else if (keyVal == values[1])
				return obj.ToObject<T2>(serializer);
			else if (keyVal == values[2])
				return obj.ToObject<T3>(serializer);
			else if (keyVal == values[3])
				return obj.ToObject<T4>(serializer);

			throw new InvalidOperationException("json id value isn't in value range");
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}
}
