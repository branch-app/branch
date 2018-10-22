namespace Halo4 {
	export interface ServiceRecordResponse extends Branch.Response {
		cacheInfo: Branch.CacheInfo;

		dateFidelity: Common.DateFidelity;
		lastPlayed: Date;
		firstPlayed: Date;
		identity: ServiceRecord.Identity;
		favoriteWeapon: ServiceRecord.FavoriteWeapon;
		specializations: ServiceRecord.Specialization[];
		gameModes: ServiceRecord.GameModes;
		currentRank: ServiceRecord.Rank;
		nextRank?: ServiceRecord.Rank;
		skillRanks: Common.SkillRank[];
		topMedals: Common.MedalRecord[];
		xp: number;
		spartanPoints: number;
		totalGamesStarted: number;
		totalMedalsEarned: number;
		totalGameplay: string;
		totalChallengesCompleted: number;
		totalLoadoutItemsPurchased: number;
		totalCommendationProgress: number;
	}
}
