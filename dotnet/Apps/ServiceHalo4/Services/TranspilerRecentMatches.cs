using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Clients.Identity;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Enums.Halo4;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Exceptions;
using Branch.Packages.Models.Halo4.RecentMatches;
using Ext = Branch.Apps.ServiceHalo4.Models.Waypoint;
using ExtRM = Branch.Apps.ServiceHalo4.Models.Waypoint.RecentMatches;
using Int = Branch.Packages.Models.Halo4;
using IntRM = Branch.Packages.Models.Halo4.RecentMatches;

namespace Branch.Apps.ServiceHalo4.Services
{
	public partial class Transpiler
	{
		public Int.RecentMatchesResponse RecentMatches(Ext.RecentMatchesResponse src, uint newCount)
		{
			var originalCount = newCount - 1;
			var HasMoreMatches = src.Games.Length == newCount;

			return new Int.RecentMatchesResponse
			{
				DateFidelity = (DateFidelity) src.DateFidelity,
				HasMoreMatches = HasMoreMatches,
				Matches = recentMatch(src.Games.Take((int) originalCount).ToArray()),
			};
		}

		private IntRM.RecentMatch[] recentMatch(ExtRM.RecentMatch[] src)
		{
			if (src.Length == 0)
				return new IntRM.RecentMatch[0];

			var output = new List<IntRM.RecentMatch>();

			foreach (var match in src)
			{
				IntRM.RecentMatch final;

				switch(match.ModeId)
				{
					case 4:
						{
							var casted = match as ExtRM.CampaignRecentMatch;
							final = new IntRM.RecentCampaignMatch
							{
								SinglePlayer = casted.SinglePlayer,
								Difficulty = (Difficulty)casted.Difficulty,
								MissionId = casted.Mission,
								Duration = casted.Duration,
								Map = recentMatchCampaignMap(casted),
								SkullIds = casted.SkullIds,
							};
							break;
						}

					case 5:
						{
							var casted = match as ExtRM.SpartanOpsRecentMatch;
							final = new IntRM.RecentSpartanOpsMatch
							{
								SinglePlayer = casted.SinglePlayer,
								Difficulty = (Difficulty) casted.Difficulty,
								Duration = casted.Duration,
								SeasonId = casted.SeasonId,
								Episode = recentMatchSpartanOpsEpisode(casted),
								Chapter = recentMatchSpartanOpsChapter(casted),
							};
							break;
						}

					case 3:
						{
							var casted = match as ExtRM.WarGamesRecentMatch;
							final = new IntRM.RecentCustomGamesMatch
							{
								GameVariant = recentMatchMultiplayerGameVariant(casted),
								FeaturedStat = recentMatchMultiplayerFeaturedStat(casted),
								Map = recentMatchMultiplayerMap(casted),
								TotalMedals = casted.TotalMedals,
							};
							break;
						}

					case 6:
						{
							var casted = match as ExtRM.WarGamesRecentMatch;
							final = new IntRM.RecentWarGamesMatch
							{
								GameVariant = recentMatchMultiplayerGameVariant(casted),
								FeaturedStat = recentMatchMultiplayerFeaturedStat(casted),
								Map = recentMatchMultiplayerMap(casted),
								Playlist = recentMatchWarGamesPlaylist(casted),
								TotalMedals = casted.TotalMedals,
							};
							break;
						}

					default:
						throw new BranchException(
							"unknown_game_mode_id",
							new Dictionary<string, object>{ {"ModeId", match.ModeId} }
						);
				}

				final.Id = match.Id;
				final.GameMode = (GameMode) match.ModeId;
				final.PersonalScore = match.PersonalScore;
				final.Completed = match.Completed;
				final.Result = (MatchResult) match.Result;
				final.TopMedalIds = match.TopMedalIds;
				final.EndDate = match.EndDateUtc;

				output.Add(final);
			}

			return output.ToArray();
		}

		private IntRM.CampaignMap recentMatchCampaignMap(ExtRM.CampaignRecentMatch src)
		{
			return new IntRM.CampaignMap
			{
				Id = src.MapId,
				Name = src.MapName,
				ImageUrl = Url(src.MapImageUrl),
			};
		}

		private IntRM.SpartanOpsEpisode recentMatchSpartanOpsEpisode(ExtRM.SpartanOpsRecentMatch src)
		{
			return new IntRM.SpartanOpsEpisode
			{
				Id = src.EpisodeId,
				Name = src.EpisodeName,
			};
		}

		private IntRM.SpartanOpsChapter recentMatchSpartanOpsChapter(ExtRM.SpartanOpsRecentMatch src)
		{
			return new IntRM.SpartanOpsChapter
			{
				Id = src.ChapterId,
				Name = src.ChapterName,
				Number = src.ChapterNumber,
			};
		}

		private IntRM.GameVariant recentMatchMultiplayerGameVariant(ExtRM.WarGamesRecentMatch src)
		{
			return new IntRM.GameVariant
			{
				BaseId = src.BaseVariantId,
				BaseImageUrl = Url(src.BaseVariantImageUrl),
				Name = src.VariantName,
			};
		}

		private IntRM.FeaturedStat recentMatchMultiplayerFeaturedStat(ExtRM.WarGamesRecentMatch src)
		{
			return new IntRM.FeaturedStat
			{
				Name = src.FeaturedStatName,
				Value = src.FeaturedStatValue,
			};
		}

		private IntRM.MultiplayerMap recentMatchMultiplayerMap(ExtRM.WarGamesRecentMatch src)
		{
			return new IntRM.MultiplayerMap
			{
				Id = src.MapId,
				ImageUrl = Url(src.MapImageUrl),
				VariantName = src.MapVariantName,
			};
		}

		private IntRM.WarGamesPlaylist recentMatchWarGamesPlaylist(ExtRM.WarGamesRecentMatch src)
		{
			return new IntRM.WarGamesPlaylist
			{
				Id = src.MapId,
				Name = src.PlaylistName,
			};
		}
	}
}
