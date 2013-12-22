﻿using System.Linq;
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

		public static PlaylistModel GetPlaylist(int playlistId)
		{
			return GlobalStorage.H4WaypointManager.Playlists.Playlists.FirstOrDefault(p => p.Id == playlistId);
		}

		public static MetadataModels.Map GetMapInfo(int mapId)
		{
			return GlobalStorage.H4WaypointManager.Metadata.MapsMetadata.Maps.FirstOrDefault(m => m.Id == mapId);
		}

		public static MetadataModels.Difficulty GetDifficultyInfo(int difficultyId)
		{
			return
				GlobalStorage.H4WaypointManager.Metadata.DifficultiesMetadata.Difficulties.FirstOrDefault(d => d.Id == difficultyId);
		}

		public static MetadataModels.GameBaseVariant GetGameVariant(int baseId)
		{
			return
				GlobalStorage.H4WaypointManager.Metadata.GameBaseVariantsMetadata.GameBaseVariants.FirstOrDefault(
					g => g.Id == baseId);
		}

		public static MetadataModels.SpartanOpsChapter GetChapter(int chapterId)
		{
			return (from season in GlobalStorage.H4WaypointManager.Metadata.SpartanOpsMetadata.Seasons
				from episode in season.Episodes
				from chapter in episode.Chapters
				select chapter).FirstOrDefault();
		}
	}
}