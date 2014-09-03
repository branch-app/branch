using System.Collections.Generic;
using Branch.Core.Game.Halo4.Models.Branch;
using Branch.Core.Game.Halo4.Models._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class CsrViewModel : Base
	{
		public CsrViewModel(ServiceRecord serviceRecord, IEnumerable<PlaylistOrientationEntity> playlistOrientations) :
			base(serviceRecord)
		{
			PlaylistOrientations = playlistOrientations;
		}

		public IEnumerable<PlaylistOrientationEntity> PlaylistOrientations { get; set; }
	}
}
