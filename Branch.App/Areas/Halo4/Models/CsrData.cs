using System.Collections.Generic;
using Branch.Models.Services.Halo4.Branch;
using Branch.Models.Services.Halo4._343.Responses;

namespace Branch.App.Areas.Halo4.Models
{
	public class CsrData : Base
	{
		public CsrData(ServiceRecord serviceRecord, IEnumerable<PlaylistOrientationEntity> playlistOrientations)
		{
			ServiceRecord = serviceRecord;
			PlaylistOrientations = playlistOrientations;
		}

		public IEnumerable<PlaylistOrientationEntity> PlaylistOrientations { get; set; }
	}
}
