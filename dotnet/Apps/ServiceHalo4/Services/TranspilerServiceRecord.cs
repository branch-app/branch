using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Clients.Identity;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Enums.Halo4;
using Branch.Packages.Enums.ServiceIdentity;
using Crpc.Exceptions;
using Ext = Branch.Apps.ServiceHalo4.Models.Waypoint;
using ExtSR = Branch.Apps.ServiceHalo4.Models.Waypoint.ServiceRecord;
using Int = Branch.Packages.Models.Halo4;
using IntSR = Branch.Packages.Models.Halo4.ServiceRecord;

namespace Branch.Apps.ServiceHalo4.Services
{
	public partial class Transpiler
	{
		public Int.ServiceRecordResponse ServiceRecord(Ext.ServiceRecordResponse src)
		{
			return new Int.ServiceRecordResponse
			{
				DateFidelity = (DateFidelity) src.DateFidelity,
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

				Identity = serviceRecordIdentity(src),
				FavoriteWeapon = serviceRecordFavoriteWeapon(src),
				Specializations = serviceRecordSpecializations(src),
				GameModes = serviceRecordGameModes(src),
				CurrentRank = serviceRecordCurrentRank(src),
				NextRank = serviceRecordNextRank(src),
				SkillRanks = skillRanks(src.SkillRanks),
				TopMedals = medalRecords(src.TopMedals),
			};
		}

		private IntSR.Identity serviceRecordIdentity(Ext.ServiceRecordResponse src)
		{
			return new IntSR.Identity
			{
				XUID = -1,
				ServiceTag = src.ServiceTag,
				EmblemUrl = Url(src.EmblemImageUrl),
			};
		}

		private IntSR.FavoriteWeapon serviceRecordFavoriteWeapon(Ext.ServiceRecordResponse src)
		{
			return new IntSR.FavoriteWeapon
			{
				ID = src.FavoriteWeaponId,
				Name = src.FavoriteWeaponName,
				Description = src.FavoriteWeaponDescription,
				ImageUrl = Url(src.FavoriteWeaponImageUrl),
				TotalKills = src.FavoriteWeaponTotalKills,
			};
		}

		private IntSR.Rank serviceRecordCurrentRank(Ext.ServiceRecordResponse src)
		{
			return new IntSR.Rank
			{
				ID = src.RankId,
				Name = src.RankName,
				ImageUrl = Url(src.RankImageUrl),
				StartXP = src.RankStartXP,
			};
		}

		private IntSR.Rank serviceRecordNextRank(Ext.ServiceRecordResponse src)
		{
			if (src.NextRankId == 0)
				return null;

			return new IntSR.Rank
			{
				ID = src.NextRankId,
				Name = src.NextRankName,
				ImageUrl = Url(src.NextRankImageUrl),
				StartXP = Convert.ToInt32(src.NextRankStartXP),
			};
		}

		private IntSR.Specialization[] serviceRecordSpecializations(Ext.ServiceRecordResponse src)
		{
			if (src.Specializations == null)
				return new IntSR.Specialization[0];

			return src.Specializations.Select(s => new IntSR.Specialization
			{
				ID = s.Id,
				Name = s.Name,
				Description = s.Description,
				ImageUrl = Url(s.ImageUrl),
				Level = s.Level,
				Current = s.IsCurrent,
				Completion = s.PercentComplete,
				Complete = s.Completed,
			}).ToArray();
		}

		private IntSR.GameModes serviceRecordGameModes(Ext.ServiceRecordResponse src)
		{
			var o = new IntSR.GameModes();

			if (src.GameModes == null || src.GameModes.Count() == 0)
				return o;

			foreach (var mode in src.GameModes)
			{
				switch(mode)
				{
					case Ext.ServiceRecord.CampaignMode m:
						o.Campaign = new IntSR.CampaignMode
						{
							SinglePlayerMissions = missions(m.SinglePlayerMissions),
							CoopMissions = missions(m.CoopMissions),
							TotalTerminalsVisited = m.TotalTerminalsVisited,
							NarrativeFlags = m.NarrativeFlags,
							SinglePlayerDASO = m.SinglePlayerDaso,
							SinglePlayerDifficulty = m.SinglePlayerDifficulty,
							CoopDASO = m.CoopDaso,
							CoopDifficulty = m.CoopDifficulty,

							TotalDuration = TimeSpan.Parse(m.TotalDuration),
							TotalKills = m.TotalKills,
							TotalDeaths = m.TotalDeaths,
							TotalGamesStarted = m.TotalGamesStarted,
						};
						break;
					case Ext.ServiceRecord.SpartanOpsMode m:
						o.SpartanOps = new IntSR.SpartanOpsMode
						{
							TotalSinglePlayerMissionsCompleted = (int) m.TotalSinglePlayerMissionsCompleted,
							TotalCoopMissionsCompleted = (int) m.TotalCoopMissionsCompleted,
							TotalMissionsPossible = (int) m.TotalMissionsPossible,
							TotalMedals = (int) m.TotalMedals,
							TotalGamesWon = (int) m.TotalGamesWon,

							TotalDuration = TimeSpan.Parse(m.TotalDuration),
							TotalKills = m.TotalKills,
							TotalDeaths = m.TotalDeaths,
							TotalGamesStarted = m.TotalGamesStarted,
						};
						break;
					case Ext.ServiceRecord.WarGamesMode m:
						var final = new IntSR.WarGamesMode
						{
							TotalMedals = (int) m.TotalMedals,
							TotalGamesWon = (int) m.TotalGamesWon,
							TotalGamesCompleted = (int) m.TotalGamesCompleted,
							AveragePersonalScore = (int) m.AveragePersonalScore,
							KdRatio = (double) m.KdRatio,
							TotalGameBaseVariantMedals = (int) m.TotalGameBaseVariantMedals,
							FavoriteVariant = serviceRecordFavoriteVariant(m.FavoriteVariant),

							TotalDuration = TimeSpan.Parse(m.TotalDuration),
							TotalKills = m.TotalKills,
							TotalDeaths = m.TotalDeaths,
							TotalGamesStarted = m.TotalGamesStarted,
						};

						switch(m.Id)
						{
							case 3: o.WarGames = final; break;
							case 6: o.CustomGames = final; break;
							default:
								throw new CrpcException(
									"unknown_game_mode_id",
									new Dictionary<string, object>{ {"Id", m.Id} }
								);
						}
						break;

					default:
						throw new CrpcException(
							"unknown_game_mode",
							new Dictionary<string, object>{ {"ModeType", mode.GetType()} }
						);
				}
			}

			return o;
		}

		private IntSR.FavoriteVariant serviceRecordFavoriteVariant(ExtSR.FavoriteVariant src)
		{
			if (src == null)
				return null;

			return new IntSR.FavoriteVariant
			{
				ImageUrl = Url(src.ImageUrl),
				TotalDuration = src.TotalDuration,
				TotalGamesStarted = src.TotalGamesStarted,
				TotalGamesCompleted = src.TotalGamesCompleted,
				TotalGamesWon = src.TotalGamesWon,
				TotalMedals = src.TotalMedals,
				TotalKills = src.TotalKills,
				TotalDeaths = src.TotalDeaths,
				KdRatio = src.KdRatio,
				AveragePersonalScore = src.AveragePersonalScore,
				Id = src.Id,
				Name = src.Name,
			};
		}
	}
}
