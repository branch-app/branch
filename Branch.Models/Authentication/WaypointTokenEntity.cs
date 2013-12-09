using System;

namespace Branch.Models.Authentication
{
	/// <summary>
	/// Holds a Halo Waypoint authentication response
	/// </summary>
	public class WaypointTokenEntity : BaseEntity
	{
		private const string RowKeyString = "343Auth";
		public static string FormatRowKey()
		{
			return String.Format(RowKeyString);
		}

		public WaypointTokenEntity()
		{
			SetKeys(null, null);
		}

		public override sealed void SetKeys(string partitionKey, string rowKey)
		{
			if (partitionKey == null)
				partitionKey = "Authentication";

			base.SetKeys(partitionKey, FormatRowKey());
		}

		/// <summary>
		/// Indicates the result of the request
		/// </summary>
		public int ResponseCode { get; set; }
			
		/// <summary>
		/// Actual token - compatible with X-343-Authorization-Spartan
		/// </summary>
		public string SpartanToken { get; set; }
			
		/// <summary>
		/// Identifies the user token is for
		/// </summary>
		public UserInfo UserInformation { get; set; }
		
		/// <summary>
		/// Information about the user identified by the Spartan auth
		/// </summary>
		public class UserInfo
		{
			/// <summary>
			/// Xbox Live gamertag of user
			/// </summary>
			public string Gamertag { get; set; }
			
			/// <summary>
			/// Used to track API usage
			/// </summary>
			/// <remarks>Unused</remarks>
			public string AnalyticsToken { get; set; }
		}
	}
}
