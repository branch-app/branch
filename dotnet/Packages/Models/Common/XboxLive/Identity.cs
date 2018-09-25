namespace Branch.Packages.Models.Common.XboxLive
{
	public class Identity
	{
		/// <summary>
		/// The friendly name chosen by the user.
		/// </summary>
		public string Gamertag { get; set; }

		/// <summary>
		/// The underlying ID of the user.
		/// </summary>
		public long XUID { get; set; }

		/// <summary>
		/// A string representation of the ID.
		/// </summary>
		public string XUIDStr { get { return _xuidStr ?? (_xuidStr = XUID.ToString()); } }
		private string _xuidStr;
	}
}
