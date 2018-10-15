using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Clients.Identity;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Exceptions;
using Ext = Branch.Packages.Models.External.Halo4;
using Int = Branch.Packages.Models.Halo4;

namespace Branch.Apps.ServiceHalo4.Services
{
	public partial class Transpiler
	{
		public async Task<Int.ServiceRecordResponse> ServiceRecord(Ext.ServiceRecordResponse src)
		{
			var identity = await identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = XboxLiveIdentityType.Gamertag,
				Value = src.Gamertag,
			});

			var output = new Int.ServiceRecordResponse
			{
				DateFidelity = 1,
				FirstPlayed = src.FirstPlayedUtc,
				LastPlayed = src.LastPlayedUtc,
				XP = src.XP,
				SpartanPoints = src.SpartanPoints,
				TotalGamesStarted = src.TotalGamesStarted,
				TotalMedalsEarned = src.TotalMedalsEarned,
				TotalGameplay = src.TotalGameplay,
				TotalChallengesCompleted = src.TotalChallengesCompleted,
				TotalLoadoutItemsPurchased = src.TotalLoadoutItemsPurchased,
				TotalCommendationProgress = src.TotalCommendationProgress,

				Identity = new Int.ServiceRecord.Identity
				{
					XUID = identity.XUID,
					ServiceTag = src.ServiceTag,
					EmblemUrl = Url(src.EmblemImageUrl),
				},

				FavoriteWeapon = new Int.ServiceRecord.FavoriteWeapon
				{
					ID = src.FavoriteWeaponId,
					Name = src.FavoriteWeaponName,
					Description = src.FavoriteWeaponDescription,
					ImageUrl = src.FavoriteWeaponImageUrl.AssetUrl,
					TotalKills = src.FavoriteWeaponTotalKills,
				},

				CurrentRank = new Int.ServiceRecord.Rank
				{
					ID = src.RankId,
					Name = src.RankName,
					ImageUrl = Url(src.RankImageUrl),
					StartXP = src.RankStartXP,
				},

				Specializations = src.Specializations.Select(s => new Int.ServiceRecord.Specialization
				{
					ID = s.Id,
					Name = s.Name,
					Description = s.Description,
					ImageUrl = Url(s.ImageUrl),
					Level = s.Level,
					Current = s.IsCurrent,
					Completion = s.PercentComplete,
					Complete = s.Completed,
				}).ToArray(),

				GameModes = new Int.ServiceRecord.GameModes(),
			};

			if (src.NextRankId != 0)
			{
				output.NextRank = new Int.ServiceRecord.Rank
				{
					ID = src.NextRankId,
					Name = src.NextRankName,
					ImageUrl = Url(src.NextRankImageUrl),
					StartXP = Convert.ToInt32(src.NextRankStartXP),
				};
			}

			foreach(var mode in (src.GameModes ?? new Ext.ServiceRecord.GameMode[0]))
			{
				Int.ServiceRecord.GameModeBase final = null;

				switch(mode)
				{
					case Ext.ServiceRecord.CampaignMode m:
						final = new Int.ServiceRecord.CampaignMode
						{
							// DifficultyLevels = m.DifficultyLevels,
							// SinglePlayerMissions = m.SinglePlayerMissions,
							// CoopMissions = m.CoopMissions,
							TotalTerminalsVisited = m.TotalTerminalsVisited,
							NarrativeFlags = m.NarrativeFlags,
							SinglePlayerDASO = m.SinglePlayerDaso,
							SinglePlayerDifficulty = m.SinglePlayerDifficulty,
							CoopDASO = m.CoopDaso,
							CoopDifficulty = m.CoopDifficulty,
						};
						break;
					case Ext.ServiceRecord.SpartanOpsMode m:
						final = new Int.ServiceRecord.SpartanOpsMode
						{
							TotalSinglePlayerMissionsCompleted = m.TotalSinglePlayerMissionsCompleted,
							TotalCoopMissionsCompleted = m.TotalCoopMissionsCompleted,
							TotalMissionsPossible = m.TotalMissionsPossible,
							TotalMedals = m.TotalMedals,
							TotalGamesWon = m.TotalGamesWon,
						};
						break;
					case Ext.ServiceRecord.WarGamesMode m:
						final = new Int.ServiceRecord.WarGamesMode
						{
							TotalMedals = m.TotalMedals,
							TotalGamesWon = m.TotalGamesWon,
							TotalGamesCompleted = m.TotalGamesCompleted,
							AveragePersonalScore = m.AveragePersonalScore,
							KdRatio = m.KdRatio,
							TotalGameBaseVariantMedals = m.TotalGameBaseVariantMedals,
							// FavoriteVariant = m.FavoriteVariant,
						};
						break;

					default:
						throw new BranchException(
							"unknown_game_mode",
							new Dictionary<string, object>{ {"ModeType", mode.GetType()} }
						);
				}

				final.TotalDuration = TimeSpan.Parse(mode.TotalDuration);
				final.TotalKills = mode.TotalKills;
				final.TotalDeaths = mode.TotalDeaths;
				final.TotalGamesStarted = mode.TotalGamesStarted;

				switch(mode.Id)
				{
					case 3: output.GameModes.WarGames = final as Int.ServiceRecord.WarGamesMode; break;
					case 4: output.GameModes.Campaign = final as Int.ServiceRecord.CampaignMode; break;
					case 5: output.GameModes.SpartanOps = final as Int.ServiceRecord.SpartanOpsMode; break;
					case 6: output.GameModes.CustomGames = final as Int.ServiceRecord.WarGamesMode; break;

					default:
						throw new BranchException(
							"unknown_game_mode",
							new Dictionary<string, object>{ {"ModeId", mode.Id} }
						);
				}
			}

			return output;
		}
	}
}
