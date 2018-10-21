using System;
using Branch.Packages.Converters;
using Branch.Packages.Enums.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.RecentMatches
{
	public class GameVariant
	{
		public int BaseId { get; set; }

		public string BaseImageUrl { get; set; }

		public string Name { get; set; }
	}
}
