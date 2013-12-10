using System.Collections.Generic;
using Newtonsoft.Json;

namespace Branch.Models.Services.Halo4._343.DataModels
{
	public class MetadataModels
	{
		#region Achievements
		public class AchievementMetadata
		{
			public IList<Achievement> Achievements { get; set; }
		}
		public class Achievement
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string LockedDescription { get; set; }

			public string UnlockedDescription { get; set; }

			public int GamerPoints { get; set; }

			public CommonModels.ImageUrl LockedImageUrl { get; set; }

			public CommonModels.ImageUrl UnlockedImageUrl { get; set; }
		}
		#endregion

		#region Armour Group
		public class ArmorGroupMetadata
		{
			public IList<ArmorGroup> ArmourGroups { get; set; }
		}
		public class ArmorGroup
		{
			public int Id { get; set; }

			public int Order { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }

			public IList<ArmorItem> ArmourItems { get; set; }
		}
		public class ArmorItem
		{
			public int Id { get; set; }

			public int Order { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region Challenges
		public class ChallengesMetadata
		{
			public IList<ChallengeCategory> ChallengeCategories { get; set; }

			public IList<ChallengePeriod> ChallengePeriods { get; set; }
		}
		public class ChallengeCategory
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		public class ChallengePeriod
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string Namely { get; set; }
		}
		#endregion

		#region Commendations
		public class CommendationsMetadata
		{
			public IList<Commendation> Commendations { get; set; }

			public IList<CommendationCategory> CommendationCategories { get; set; }

			public IList<CommendationLevel> CommendationLevels { get; set; }
		}
		public class Commendation
		{
			public int Id { get; set; }

			public int CategoryId { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }
		}
		public class CommendationCategory
		{
			public int Id { get; set; }

			public string Name { get; set; }
		}
		public class CommendationLevel
		{
			public int CommendationId { get; set; }

			public int Level { get; set; }

			public int Ticks { get; set; }

			public int Xp { get; set; }

			public string Name { get; set; }
		}
		#endregion

		#region Damage
		public class DamageMetadata
		{
			public IList<DamageClass> DamageClasses { get; set; }

			public IList<WeaponType> WeaponTypes { get; set; }

			public IList<DamageType> DamageTypes { get; set; }
		}
		public class DamageClass
		{
			public int Id { get; set; }

			public string Name { get; set; }
		}
		public class WeaponType
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		public class DamageType
		{
			public int Id { get; set; }

			public int ClassId { get; set; }

			public int WeaponClassId { get; set; }

			public int FactionId { get; set; }

			public int Range { get; set; }

			public int Power { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region Difficulty
		public class DifficultiesMetadata
		{
			public IList<Difficulty> Difficulties { get; set; }
		}
		public class Difficulty
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region Enemies
		public class EnemiesMetadata
		{
			public IList<EnemyClass> EnemyClasses { get; set; }

			public IList<EnemyType> EnemyTypes { get; set; }

			public IList<Enemy> Enemies { get; set; }
		}
		public class EnemyClass
		{
			public int Id { get; set; }

			public string Name { get; set; }
		}
		public class EnemyType
		{
			public int Id { get; set; }

			public string Name { get; set; }
		}
		public class Enemy
		{
			public int Id { get; set; }

			public int ClassId { get; set; }

			public int TypeId { get; set; }

			public int FactionId { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region Emblems
		public class EmblemsMetadata
		{
			public IList<EmblemColor> EmblemColors { get; set; }

			public IList<EmblemForegroundShape> EmblemForegroundShapes { get; set; }

			public IList<EmblemBackgroundShape> EmblemBackgroundShapes { get; set; }
		}
		public class EmblemColor
		{
			public string Id { get; set; }

			public string Name { get; set; }

			[JsonProperty("ColorARGB")]
			public string ColorArgb { get; set; }
		}
		public class EmblemForegroundShape
		{
			public string Id { get; set; }

			public int Order { get; set; }

			public string Name { get; set; }
		}
		public class EmblemBackgroundShape
		{
			public string Id { get; set; }

			public int Order { get; set; }

			public string Name { get; set; }
		}
		#endregion

		#region Factions
		public class FactionsMetadata
		{
			public IList<Faction> Factions { get; set; }
		}
		public class Faction
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region GameModes
		public class GameModesMetadata
		{
			public IList<GameMode> GameModes { get; set; }
		}
		public class GameMode
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public string ArticleId { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region Game Base Varients
		public class GameBaseVariantsMetadata
		{
			public IList<GameBaseVariant> GameBaseVariants { get; set; }
		}
		public class GameBaseVariant
		{
			public int Id { get; set; }

			[JsonProperty("KDRelevant")]
			public bool KdRelevant { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public string FeaturedStatName { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region Maps
		public class MapsMetadata
		{
			public IList<Map> Maps { get; set; }
		}
		public class Map
		{
			public int Id { get; set; }

			public int GameModeId { get; set; }

			public string ArticleId { get; set; }

			public int Order { get; set; }

			public int? Mission { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region Medals
		public class MedalsMetadata
		{
			public IList<Medal> Medals { get; set; }

			public IList<MedalClass> MedalClasses { get; set; }

			public IList<MedalTier> MedalTiers { get; set; }
		}
		public class Medal
		{
			public int Id { get; set; }

			public int TierId { get; set; }

			public int ClassId { get; set; }

			public int GameBaseVariantId { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}

		public class MedalClass
		{
			public int Id { get; set; }

			public string Name { get; set; }
		}

		public class MedalTier
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }
		}
		#endregion

		#region NarrativeFlags
		public class NarrativeFlagsMetadata
		{
			public IList<NarrativeFlag> NarrativeFlags { get; set; }
		}
		public class NarrativeFlag
		{
			public int Id { get; set; }

			public string ArticleId { get; set; }

			public string Name { get; set; }
		}
		#endregion

		#region Player Upgrades
		public class PlayerUpgradesMetadata
		{
			public IList<ArmorAbility> ArmorAbilities { get; set; }

			public IList<SupportUpgrade> SupportUpgrades { get; set; }

			public IList<TacticalPackage> TacticalPackages { get; set; }
		}
		public class ArmorAbility
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		public class SupportUpgrade
		{
			public int Id { get; set; }

			public int Order { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		public class TacticalPackage
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}

		#endregion

		#region Playlists
		public class PlaylistsMetadata
		{
			public IList<Playlist> Playlists { get; set; }
		}
		public class Playlist
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }

			public int ModeId { get; set; }

			public string ModeName { get; set; }

			public int MaxPartySize { get; set; }

			public int MaxLocalPlayers { get; set; }

			public bool IsFreeForAll { get; set; }

			public IList<CommonModels.GameVariant> RelatedGameVariants { get; set; }

			public IList<CommonModels.MapVariant> RelatedMapVariants { get; set; }

			public string EffectiveOn { get; set; }

			public string EffectiveUntil { get; set; }
		}
		#endregion

		#region Poses
		public class PosesMetadata
		{
			// ReSharper disable once MemberHidesStaticFromOuterClass
			public IList<Poses> Poses { get; set; }
		}
		public class Poses
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region PromotionTypes
		public class PromotionTypesMetadata
		{
			public IList<PromotionType> PromotionTypes { get; set; }
		}
		public class PromotionType
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }
		}
		#endregion

		#region Ranks
		public class RanksMetadata
		{
			public IList<Rank> Ranks { get; set; }
		}
		public class Rank
		{
			public int Id { get; set; }

			public int Credits { get; set; }

			public int CompletedSpecializations { get; set; }

			public int StartXp { get; set; }

			public int EndXp { get; set; }

			public string Name { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region RankAwards
		public class RankAwardsMetadata
		{
			public IList<RankAward> RankAwards { get; set; }
		}
		public class RankAward
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }

			public int SpecializationId { get; set; }

			// ReSharper disable once MemberHidesStaticFromOuterClass
			public int SpecializationLevel { get; set; }
		}
		#endregion

		#region Skulls
		public class SkullsMetadata
		{
			public IList<Skull> Skulls { get; set; }
		}
		public class Skull
		{
			public int Id { get; set; }

			public int Order { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		#endregion

		#region SpartanOps
		public class SpartanOpsMetadata
		{
			public int SeasonsReleasedToDate { get; set; }

			public int ChaptersCurrentlyAvailable { get; set; }

			public int BumperType { get; set; }

			public int CurrentSeason { get; set; }

			public int CurrentEpisode { get; set; }

			public IList<Season> Seasons { get; set; }
		}
		public class Episode
		{
			public int Id { get; set; }

			public string Title { get; set; }

			public string Description { get; set; }

			public IList<Video> Videos { get; set; }

			public IList<Chapter> Chapters { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		public class Season
		{
			public int Id { get; set; }

			public IList<Episode> Episodes { get; set; }

			public object Epilogue { get; set; }
		}
		public class Chapter
		{
			public int Id { get; set; }

			public int Number { get; set; }

			public string Title { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}
		public class Video
		{
			public string Language { get; set; }

			public string Folder { get; set; }

			public string WebFileName { get; set; }

			public string ConsoleFileName { get; set; }

			public string MobileFileName { get; set; }

			public string IosFileName { get; set; }

			public string IosSuffix { get; set; }

			public string Mp4FileName { get; set; }

			public string Mp4Suffix { get; set; }
		}
		#endregion

		#region  Specializations
		public class SpecializationsMetadata
		{
			public IList<Specialization> Specializations { get; set; }

			public IList<SpecializationLevel> SpecializationLevels { get; set; }
		}
		public class Specialization
		{
			public int Id { get; set; }

			[JsonProperty("MaxSpecializationXP")]
			public int MaxSpecializationXp { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public CommonModels.ImageUrl ImageUrl { get; set; }
		}

		public class SpecializationLevel
		{
			public int Id { get; set; }

			public int SpecializationId { get; set; }

			public int Level { get; set; }

			public int StartXp { get; set; }

			public string Name { get; set; }
		}
		#endregion

		#region Team Appearances
		public class TeamAppearancesMetadata
		{
			public IList<TeamAppearanceSetting> TeamAppearanceSettings { get; set; }
		}
		public class TeamAppearanceSetting
		{
			public int Id { get; set; }

			[JsonProperty("PrimaryRGBA")]
			public int PrimaryRgba { get; set; }

			[JsonProperty("PrimaryRGB")]
			public string PrimaryRgb { get; set; }

			[JsonProperty("SecondaryRGBA")]
			public int SecondaryRgba { get; set; }

			[JsonProperty("SecondaryRGB")]
			public string SecondaryRgb { get; set; }

			public string Name { get; set; }

			public string ForegroundShapeId { get; set; }

			public string ForegroundPrimaryColor { get; set; }

			public string ForegroundSecondaryColor { get; set; }

			public string BackgroundShapeId { get; set; }

			public string BackgroundColorId { get; set; }

			public CommonModels.ImageUrl EmblemUrl { get; set; }
		}
		#endregion
	}
}
