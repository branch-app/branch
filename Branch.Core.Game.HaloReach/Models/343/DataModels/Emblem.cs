using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Emblem
	{
		[JsonProperty("background_index")]
		public int BackgroundIndex { get; set; }

		[JsonProperty("change_colors")]
		public List<int> ChangeColours { get; set; }

		[JsonProperty("flags")]
		public int Flags { get; set; }

		[JsonProperty("foreground_index")]
		public int ForegroundIndex { get; set; }
	}
}
