using Branch.Core.Game.HaloReach.JsonConverters;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class Team
	{
		[JsonConverter(typeof(TickValueConverter<int>))]
		public TickValue<int>[] DeathsOverTime { get; set; }

		public bool Exists { get; set; }

		public int Index { get; set; }

		[JsonConverter(typeof(TickValueConverter<int>))]
		public TickValue<int>[] KillsOverTime { get; set; }

		[JsonConverter(typeof(TickValueConverter<int>))]
		public TickValue<int>[] MedalsOverTime { get; set; }

		public int MetagameScore { get; set; }

		public int Score { get; set; }

		public int Standing { get; set; }

		public int TeamTotalAssists { get; set; }

		public int TeamTotalBetrayals { get; set; }

		public int TeamTotalDeaths { get; set; }

		[JsonProperty("TeamTotalGameVariantCustomStat_1")]
		public int TeamTotalGameVariantCustomStat1 { get; set; }

		[JsonProperty("TeamTotalGameVariantCustomStat_2")]
		public int TeamTotalGameVariantCustomStat2 { get; set; }

		[JsonProperty("TeamTotalGameVariantCustomStat_3")]
		public int TeamTotalGameVariantCustomStat3 { get; set; }

		[JsonProperty("TeamTotalGameVariantCustomStat_4")]
		public int TeamTotalGameVariantCustomStat4 { get; set; }

		public int TeamTotalKills { get; set; }

		public int TeamTotalMedals { get; set; }

		public int TeamTotalSuicides { get; set; }
	}
}
