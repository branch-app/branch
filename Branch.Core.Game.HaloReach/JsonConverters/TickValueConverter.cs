using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Branch.Core.Game.HaloReach.Models._343.DataModels;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.JsonConverters
{
	public class TickValueConverter<T> : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Array);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var ticks = new List<TickValue<T>>();
			foreach (var x in serializer.Deserialize<int[][]>(reader))
			{
				try
				{
					ticks.Add(new TickValue<T>
					{
						Tick = x[0],
						Value = (T)Convert.ChangeType(x[1], typeof(T), CultureInfo.InstalledUICulture.NumberFormat)
					});
				}
				catch (InvalidCastException)
				{
					Trace.TraceError("[TickValueConverter] Unable to cast type {0} to Value {1}", typeof(T), x[1]);
				}
			}

			return ticks.ToArray();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public T ValueType;
	}
}
