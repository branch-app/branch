using System;

namespace Branch.Models.Authentication
{
	/// <summary>
	///     Holds a Halo Waypoint authentication response
	/// </summary>
	public class WaypointTokenEntity : BaseEntity
	{
		private const string RowKeyString = "343Auth";

		public WaypointTokenEntity()
		{
			SetKeys(null, null);
		}

		/// <summary>
		///     Actual token - compatible with X-343-Authorization-Spartan
		/// </summary>
		public string SpartanToken { get; set; }

		public static string FormatRowKey()
		{
			return String.Format(RowKeyString);
		}

		public override sealed void SetKeys(string partitionKey, string rowKey)
		{
			if (partitionKey == null)
				partitionKey = "Authentication";

			base.SetKeys(partitionKey, FormatRowKey());
		}
	}
}