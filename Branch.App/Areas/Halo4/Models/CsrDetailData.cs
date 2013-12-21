using Branch.Models.Services.Halo4.Branch;
using Branch.Models.Services.Halo4._343.DataModels;
using Branch.Models.Services.Halo4._343.Responses;
namespace Branch.App.Areas.Halo4.Models
{
	public class CsrDetailData : Base
	{
		public CsrDetailData(ServiceRecord serviceRecord, PlaylistOrientationEntity playlistOrientation, CommonModels.CurrentSkillRank currentSkillRank)
		{
			ServiceRecord = serviceRecord;
			PlaylistOrientation = playlistOrientation;
			CurrentSkillRank = currentSkillRank;
		}

		public PlaylistOrientationEntity PlaylistOrientation { get; set; }

		public CommonModels.CurrentSkillRank CurrentSkillRank { get; set; }
	}
}
