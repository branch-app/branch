namespace Branch.Packages.Models.Common.Config
{
	public class BranchServiceConfig
	{
		/// <summary>
		/// The URL to connect to the service.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// The key to connect to the service.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// The (optional) timeout when connecting to the service.
		/// </summary>
		public int? Timeout { get; set; } = 1000;
	}
}
