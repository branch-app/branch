using Branch.Core.Game.HaloReach.JsonConverters;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class PlayerGameDetails
	{
		public KeyValue<EventAggregate>[] AiEventAggregates { get; set; }

		public int Assists { get; set; }

		public double AvgDeathDistanceMeters { get; set; }

		public double AvgKillDistanceMeters { get; set; }

		public int Betrayals { get; set; }

		[JsonProperty("DNF")]
		public bool DidNotFinish { get; set; }

		public int Deaths { get; set; }

		[JsonConverter(typeof(TickValueConverter<int>))]
		public TickValue<int>[] DeathsOverTime { get; set; }

		public int Headshots { get; set; }

		public int IndividualStandingWithNoRegardForTeams { get; set; }

		public bool IsGuest { get; set; }

		public int KilledMostByCount { get; set; }

		public int KilledMostCount { get; set; }

		public int Kills { get; set; }

		[JsonConverter(typeof(TickValueConverter<int>))]
		public TickValue<int>[] KillsOverTime { get; set; }

		[JsonConverter(typeof(TickValueConverter<int>))]
		public TickValue<int>[] MedalsOverTime { get; set; }

		public int MultiMedalCount { get; set; }

		public int OtherMedalCount { get; set; }

		public int PlayerDataIndex { get; set; }

		public Player PlayerDetail { get; set; }

		public string PlayerEmblemUrl { get; set; }

		public string PlayerKilledByMost { get; set; }

		public string PlayerKilledMost { get; set; }

		[JsonConverter(typeof(TickValueConverter<int>))]
		public TickValue<int>[] PointsOverTime { get; set; }

		public int Rating { get; set; }

		public int Score { get; set; }

		public KeyValue<int>[] SpecificMedalCounts { get; set; }

		public int SpreeMedalCount { get; set; }

		public int Standing { get; set; }

		public int StyleMedalCount { get; set; }

		public int Suicides { get; set; }

		public int Team { get; set; }

		public int TeamScore { get; set; }

		public int TotalMedalCount { get; set; }

		public int UniqueMultiMedalCount { get; set; }

		public int UniqueOtherMedalCount { get; set; }

		public int UniqueSpreeMedalCount { get; set; }

		public int UniqueStyleMedalCount { get; set; }

		public int UniqueTotalMedalCount { get; set; }

		public WeaponCarnageReport[] WeaponCarnageReport { get; set; }
	}
}
