using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Branch.Packages.Enums.Halo4
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DateFidelity
	{
		[EnumMember(Value = "rough")]
		Rough = 1,

		[EnumMember(Value = "exact")]
		Exact = 2,
	}
}
