using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Branch.Packages.Enums.Halo4
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum GameMode
	{
		[EnumMember(Value = "war-games")]
		WarGames = 3,

		[EnumMember(Value = "campaign")]
		Campaign = 4,

		[EnumMember(Value = "spartan-ops")]
		SpartanOps = 5,

		[EnumMember(Value = "custom-games")]
		CustomGames = 6,
	}
}
