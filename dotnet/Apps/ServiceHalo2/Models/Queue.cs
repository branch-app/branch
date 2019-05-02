using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Branch.Packages.Models.Common.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Branch.Packages.Converters;
using System;

namespace Branch.Apps.ServiceHalo2.Models
{
	public class QueueEventTypes
	{
		public const string CacheServiceRecord = "cache_service_record";
		public const string CacheRecentMatches = "cache_recent_matches";
		public const string CacheMatch = "cache_match";
	}

	[JsonConverter(
		typeof(TypePayloadConverter<QueueEvent, QueueEventPayload>),
		new object[]
		{
			new object[]
			{
				new object[] { QueueEventTypes.CacheServiceRecord, typeof(ServiceRecordPayload) },
				new object[] { QueueEventTypes.CacheRecentMatches, typeof(MatchHistoryPayload) },
				new object[] { QueueEventTypes.CacheMatch,         typeof(MatchPayload) },
			},
		}
	)]
	public class QueueEvent : ITypePayload<QueueEventPayload>
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;

		[JsonProperty("event_identifier")]
		public string EventIdentifier { get; set; } = Ksuid.Ksuid.Generate("h2quevt").ToString();

		[JsonProperty("payload")]
		public QueueEventPayload Payload { get; set; }
	}

	public abstract class QueueEventPayload { }

	public class ServiceRecordPayload : QueueEventPayload
	{
		[JsonProperty("gamertag")]
		public string Gamertag { get; set; }
	}

	public class MatchHistoryPayload : QueueEventPayload
	{
		[JsonProperty("gamertag")]
		public string Gamertag { get; set; }

		[JsonProperty("page")]
		public int Page { get; set; }

		[JsonProperty("last_page")]
		public bool LastPage { get; set; }
	}

	public class MatchPayload : QueueEventPayload
	{
		[JsonProperty("match_id")]
		public int MatchId { get; set; }
	}
}
