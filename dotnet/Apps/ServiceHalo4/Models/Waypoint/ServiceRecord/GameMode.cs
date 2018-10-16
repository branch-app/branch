using System;
using System.Collections.Generic;
using Branch.Packages.Converters;
using Branch.Apps.ServiceHalo4.Models.Waypoint.Common;
using Newtonsoft.Json;

namespace Branch.Apps.ServiceHalo4.Models.Waypoint.ServiceRecord
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
