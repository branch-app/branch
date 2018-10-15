using System;
using System.Collections.Generic;
using Branch.Packages.Converters;
using Branch.Packages.Models.External.Halo4.Common;
using Newtonsoft.Json;

namespace Branch.Packages.Models.External.Halo4.ServiceRecord
{
	[JsonConverter(
		typeof(IdToAbstractConverter<CampaignMode, SpartanOpsMode, WarGamesMode, WarGamesMode>),
		new object[] { "PresentationId", new string[] { "1", "2", "3", "4" } }
	)]
	public abstract class GameMode
	{
		public string TotalDuration { get; set; }

		public int TotalKills { get; set; }

		public int TotalDeaths { get; set; }

		public int PresentationId { get; set; }

		public int Id { get; set; }

		public string Name { get; set; }

		public int TotalGamesStarted { get; set; }
	}
}
