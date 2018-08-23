namespace Branch.Packages.Models.XboxLive
{
	public class ProfileUser
	{
		public long ID { get; set; }

		public long HostID { get; set; }

		public ProfileUserSetting[] Settings { get; set; }

		public bool IsSponsoredUser { get; set; }
	}
}
