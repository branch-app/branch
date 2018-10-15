using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Branch.Packages.Enums.Halo4
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Result
	{
		[EnumMember(Value = "lost")]
		Lost,

		[EnumMember(Value = "draw")]
		Draw,

		[EnumMember(Value = "won")]
		Won,
	}
}
