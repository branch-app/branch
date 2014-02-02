using Branch.Models.Services.Halo4.Branch;
using Branch.Models.Services.Halo4._343.DataModels;
using Branch.Models.Services.Halo4._343.Responses;
namespace Branch.App.Areas.Halo4.Models
{
	public class CsrDetailViewModel : Base
	{
		public CsrDetailViewModel(ServiceRecord serviceRecord, PlaylistOrientationEntity playlistOrientation, CommonModels.CurrentSkillRank currentSkillRank, MetadataModels.Playlist playlist) :
			base(serviceRecord)
		{
			Playlist = playlist;
			PlaylistOrientation = playlistOrientation;
			CurrentSkillRank = currentSkillRank;
		}

		public MetadataModels.Playlist Playlist { get; set; }

		public PlaylistOrientationEntity PlaylistOrientation { get; set; }

		public CommonModels.CurrentSkillRank CurrentSkillRank { get; set; }
	}
}
