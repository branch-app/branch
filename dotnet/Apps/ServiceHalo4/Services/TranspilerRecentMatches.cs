using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Clients.Identity;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Enums.Halo4;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Exceptions;
using Ext = Branch.Apps.ServiceHalo4.Models.Waypoint;
using ExtSR = Branch.Apps.ServiceHalo4.Models.Waypoint.RecentMatches;
using Int = Branch.Packages.Models.Halo4;
// using IntSR = Branch.Packages.Models.Halo4.ServiceRecord;

namespace Branch.Apps.ServiceHalo4.Services
{
	public partial class Transpiler
	{
		public Int.RecentMatchesResponse RecentMatches(Ext.RecentMatchesResponse src, uint newCount)
		{
			var originalCount = newCount - 1;
			var HasMoreMatches = src.Games.Length == newCount;

			return new Int.RecentMatchesResponse
			{
				DateFidelity = (DateFidelity) src.DateFidelity,
				HasMoreMatches = HasMoreMatches,
				// Matches = src.Games.Take(originalCount).Select(g => ),
			};
		}
	}
}
