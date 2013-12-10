using System;
using Newtonsoft.Json;

namespace Branch.Models.Services.Halo4._343.DataModels
{
	public class ChallengeModel
	{
		public string Name { get; set; }

		public string Description { get; set; }

		[JsonProperty("CategoryId")]
		public Enums.ChallengeCategory Category { get; set; }

		public string CategoryName { get; set; }

		public int ChallengeIndex { get; set; }

		public int PeriodId { get; set; }

		public DateTime BeginDate { get; set; }

		public DateTime EndDate { get; set; }

		public int RequiredCount { get; set; }

		public string GameModeName { get; set; }

		public int XpReward { get; set; }

		public int RequiredSkulls { get; set; }

		public bool Completed { get; set; }

		public int Progress { get; set; }
	}
}
