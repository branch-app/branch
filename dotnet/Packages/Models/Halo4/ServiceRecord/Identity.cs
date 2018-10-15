using Branch.Packages.Models.External.Halo4;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public class Identity
	{
		public long XUID { get; set; }

		public string ServiceTag { get; set; }

		public Emblem Emblem { get; set; }
	}
}
