using System;
using Branch.Packages.Converters;
using Branch.Packages.Enums.Halo4;
using Newtonsoft.Json;

namespace Branch.Packages.Models.Halo4.RecentMatches
{
	public class MultiplayerMap
	{
		public int Id { get; set; }

		public string VariantName { get; set; }

		public string ImageUrl { get; set; }
	}
}
