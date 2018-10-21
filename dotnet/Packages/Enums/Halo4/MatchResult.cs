using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Branch.Packages.Enums.Halo4
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum MatchResult
	{
		[EnumMember(Value = "loss")]
		Loss = 0,

		[EnumMember(Value = "tie")]
		Tie = 1,

		[EnumMember(Value = "won")]
		Won = 2,
	}
}
