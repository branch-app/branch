using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Branch.Packages.Enums.Halo4
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DateFidelity
	{
		[EnumMember(Value = "by-day")]
		ByDay = 1,
	}
}
