namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public abstract class GameModeBase
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
