using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Branch.Packages.Enums.ServiceIdentity
{
	public enum XboxLiveIdentityType
	{
		[EnumMember(Value = "xuid")]
		Xuid,
		[EnumMember(Value = "gamertag")]
		Gamertag,
	}
}
