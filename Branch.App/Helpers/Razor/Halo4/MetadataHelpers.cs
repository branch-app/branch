using System.Linq;
using Branch.Models.Services.Halo4._343.DataModels;

namespace Branch.App.Helpers.Razor.Halo4
{
	public class MetadataHelpers
	{
		public static int CommendationMaxLevel(int commendationId)
		{
			MetadataModels.CommendationLevel commendationFinal = null;

			foreach (
				var commendationLevel in
					GlobalStorage.H4WaypointManager.Metadata.CommendationsMetadata.CommendationLevels.Where(
						c => c.CommendationId == commendationId))
				commendationFinal = commendationLevel;

			return (commendationFinal == null) ? -1 : commendationFinal.Level + 1;
		}
	}
}