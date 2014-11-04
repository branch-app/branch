using Branch.Models.Sql;

namespace Branch.App.Areas.Identity.Models
{
	public class HomeIdentityViewModel
	{
		public HomeIdentityViewModel(BranchIdentity branchIdentity)
		{
			BranchIdentity = branchIdentity;
		}

		public BranchIdentity BranchIdentity { get; set; }
	}
}