using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Branch.Packages.Enums.External.Halo4
{
	public enum ResponseCode
	{
		Okay = 0,
		PlayerFound = 1,
		InvalidGamertag = 3,
		PlayerHasNotPlayedHalo4 = 4
	}
}
