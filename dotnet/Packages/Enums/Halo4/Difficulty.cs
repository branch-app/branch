using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Branch.Packages.Enums.Halo4
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Difficulty
	{
		[EnumMember(Value = "easy")]
		Easy = 0,

		[EnumMember(Value = "normal")]
		Normal = 1,

		[EnumMember(Value = "heroic")]
		Heroic = 2,

		[EnumMember(Value = "legendary")]
		Legendary = 3,
	}
}
