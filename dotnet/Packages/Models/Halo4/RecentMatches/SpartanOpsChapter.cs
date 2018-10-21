using System;
using Branch.Packages.Converters;
using Branch.Packages.Enums.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.RecentMatches
{
	public class SpartanOpsChapter
	{
		public int Id { get; set; }

		public int Number { get; set; }

		public string Name { get; set; }
	}
}
