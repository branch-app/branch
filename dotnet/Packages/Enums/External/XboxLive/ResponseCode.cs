using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Branch.Packages.Enums.External.XboxLive
{
	public enum ResponseCode
	{
		Unknown,

		XUIDInvalid = 2,
		ProfileNotFound = 28,
	}
}
