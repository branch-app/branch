using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Models
{
	public class FileShareViewModel
		: Base
	{
		public FileShareViewModel(ServiceRecord serviceRecord, FileShare fileShare)
			: base(serviceRecord)
		{
			FileShare = fileShare;
		}

		public FileShare FileShare { get; set; }
	}
}