using Branch.Packages.Enums.Halo4;

namespace Branch.Packages.Models.Halo4
{
	public class RecentMatchesResponse
	{
		public object[] Matches { get; set; }

		public bool HasMoreMatches { get; set; }

		public DateFidelity DateFidelity { get; set; }
	}
}
