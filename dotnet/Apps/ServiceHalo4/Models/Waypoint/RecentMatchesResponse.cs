using System;
using Branch.Apps.ServiceHalo4.Models.Waypoint.RecentMatches;
using Newtonsoft.Json;

namespace Branch.Apps.ServiceHalo4.Models.Waypoint
{
	public class RecentMatchesResponse : WaypointResponse
	{
		public int DateFidelity { get; set; }

		public RecentMatch[] Games { get; set; }
	}
}
