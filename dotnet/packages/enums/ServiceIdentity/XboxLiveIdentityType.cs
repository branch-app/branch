using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Branch.Packages.Enums.ServiceIdentity
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum XboxLiveIdentityType
	{
		[EnumMember(Value = "xuid")]
		Xuid,
		[EnumMember(Value = "gamertag")]
		Gamertag,
	}
}
