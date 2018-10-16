using System.Collections.Generic;
using System.Linq;
using Branch.Clients.Identity;
using ExtCom = Branch.Packages.Models.External.Halo4.Common;
using IntCom = Branch.Packages.Models.Halo4.Common;

namespace Branch.Apps.ServiceHalo4.Services
{
	public partial class Transpiler
	{
		private IdentityClient identityClient { get; }

		private Dictionary<string, string> assetMap { get; set; }

		public Transpiler(IdentityClient identity)
		{
			identityClient = identity;

			assetMap = new Dictionary<string, string>
			{
				{ "FUDMetadataAssets", "https://assets.halowaypoint.com/fud/v1/fudvideos.xml" },
				{ "H4AchievementAssets", "https://assets.halowaypoint.com/games/h4/achievements/v1/" },
				{ "H4ArmorAbilityAssets", "https://assets.halowaypoint.com/games/h4/armor-abilities/v1/" },
				{ "H4ArmorAssets", "https://assets.halowaypoint.com/games/h4/armor/v1/" },
				{ "H4ChallengeAssets", "https://assets.halowaypoint.com/games/h4/challenges/v1/" },
				{ "H4DamageTypes", "https://assets.halowaypoint.com/games/h4/damage-types/v1/" },
				{ "H4DifficultyAssets", "https://assets.halowaypoint.com/games/h4/difficulty/v1/" },
				{ "H4EmblemAssets", "https://emblems.svc.halowaypoint.com/h4/emblems/" },
				{ "H4EnemyAssets", "https://assets.halowaypoint.com/games/h4/enemies/v1/" },
				{ "H4FactionAssets", "https://assets.halowaypoint.com/games/h4/factions/v1/" },
				{ "H4GameBaseVariantAssets", "https://assets.halowaypoint.com/games/h4/game-base-variants/v1/" },
				{ "H4GameModeAssets", "https://assets.halowaypoint.com/games/h4/game-modes/v1/" },
				{ "H4MapAssets", "https://assets.halowaypoint.com/games/h4/maps/v1/" },
				{ "H4MedalAssets", "https://assets.halowaypoint.com/games/h4/medals/v1/" },
				{ "H4PlayerCardAssets", "https://assets.halowaypoint.com/games/h4/player-card/v1/" },
				{ "H4PlaylistAssets", "https://assets.halowaypoint.com/games/h4/playlists/v1/" },
				{ "H4PoseAssets", "https://assets.halowaypoint.com/games/h4/poses/v1/" },
				{ "H4RankAssets", "https://assets.halowaypoint.com/games/h4/ranks/v1/" },
				{ "H4RankAwards", "https://assets.halowaypoint.com/games/h4/rank-awards/v1/" },
				{ "H4SkullAssets", "https://assets.halowaypoint.com/games/h4/skulls/v1/" },
				{ "H4SpartanOpsBaseUrl", "https://assets.halowaypoint.com/games/h4/spartanops/v1/" },
				{ "H4SpecializationAssets", "https://assets.halowaypoint.com/games/h4/specializations/v1/" },
				{ "H4SupportUpgAssets", "https://assets.halowaypoint.com/games/h4/support-upgrades/v1/" },
				{ "H4TacticalPkgAssets", "https://assets.halowaypoint.com/games/h4/tactical-pkgs/v1/" },
				{ "H4TeamEmblemAssets", "https://assets.halowaypoint.com/games/h4/team-emblems/v1/" },
				{ "H4TerminalsBaseUrl", "https://assets.halowaypoint.com/games/h4/terminals/v1/" },
				{ "H4WeaponAssets", "https://assets.halowaypoint.com/games/h4/weapons/v1/" },
			};
		}

		public string url(ExtCom.ImageUrl src)
		{
			if (src.BaseUrl == "GameBaseVariant")
				return null;

			var urlBase = assetMap[src.BaseUrl];

			return $"{urlBase}{src.AssetUrl}";
		}

		private IntCom.DifficultyLevel difficultyLevel(ExtCom.DifficultyLevel src)
		{
			return new IntCom.DifficultyLevel
			{
				Id = src.Id,
				Name = src.Name,
				Description = src.Description,
				ImageUrl = url(src.ImageUrl),
			};
		}

		private IntCom.DifficultyLevel[] difficultyLevels(ExtCom.DifficultyLevel[] src)
		{
			if (src == null || src.Length == 0)
				return new IntCom.DifficultyLevel[0];

			return src.Select(dl => difficultyLevel(dl)).ToArray();
		}

		private IntCom.Mission mission(ExtCom.Mission src)
		{
			return new IntCom.Mission
			{
				MapId = src.MapId,
				MissionId = src.MissionId,
				Difficulty = src.Difficulty,
			};
		}

		private IntCom.Mission[] missions(ExtCom.Mission[] src)
		{
			if (src == null || src.Length == 0)
				return new IntCom.Mission[0];

			return src.Select(m => mission(m)).ToArray();
		}

		private IntCom.SkillRank skillRank(ExtCom.SkillRank src)
		{
			return new IntCom.SkillRank
			{
				CurrentSkillRank = src.CurrentSkillRank,
				Playlist = new IntCom.SkillRankPlaylist
				{
					Id = src.PlaylistId,
					Name = src.PlaylistName,
					Description = src.PlaylistDescription,
					ImageUrl = url(src.PlaylistImageUrl),
				},
			};
		}

		private IntCom.SkillRank[] skillRanks(ExtCom.SkillRank[] src)
		{
			if (src == null || src.Length == 0)
				return new IntCom.SkillRank[0];

			return src.Select(m => skillRank(m)).ToArray();
		}

		private IntCom.MedalRecord medalRecord(ExtCom.TopMedal src)
		{
			return new IntCom.MedalRecord
			{
				Id = src.Id,
				Name = src.Name,
				Description = src.Description,
				ImageUrl = url(src.ImageUrl),
				TotalMedals = src.TotalMedals,
			};
		}

		private IntCom.MedalRecord[] medalRecords(ExtCom.TopMedal[] src)
		{
			if (src == null || src.Length == 0)
				return new IntCom.MedalRecord[0];

			return src.Select(m => medalRecord(m)).ToArray();
		}
	}
}
