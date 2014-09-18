using System.Linq;
using Branch.Core.Game.HaloReach.Models._343.DataModels;
using Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Areas.Reach.Models
{
	public class FileShareRecentScreenshotsViewModel
		: Base
	{
		public FileShareRecentScreenshotsViewModel(ServiceRecord serviceRecord, FileShare fileShare)
			: base(serviceRecord)
		{
			File = fileShare.Files.First();
		}

		public File File { get; set; }
	}
}