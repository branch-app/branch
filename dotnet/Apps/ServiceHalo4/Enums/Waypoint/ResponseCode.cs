using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Branch.Apps.ServiceHalo4.Enums.Waypoint
{
	public enum ResponseCode
	{
		Okay = 0,
		Found = 1,
		NotFound = 3,
		PlayerHasNotPlayedHalo4 = 4
	}
}
