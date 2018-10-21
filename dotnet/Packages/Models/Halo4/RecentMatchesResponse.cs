using Branch.Packages.Enums.Halo4;
using Branch.Packages.Models.Halo4.RecentMatches;

namespace Branch.Packages.Models.Halo4
{
	public class RecentMatchesResponse
	{
		public RecentMatch[] Matches { get; set; }

		public bool HasMoreMatches { get; set; }

		public DateFidelity DateFidelity { get; set; }
	}
}
