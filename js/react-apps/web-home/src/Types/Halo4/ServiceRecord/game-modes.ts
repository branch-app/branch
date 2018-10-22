namespace ServiceRecord {
	export interface GameModes {
		campaign: Campaign;
		spartanOps: SpartanOps;
		warGames: WarGames;
		customGames: WarGames;
	}

	export interface Campaign {
		singlePlayerMissions: Common.Mission[];
		coopMissions: Common.Mission[];
		totalTerminalsVisited: number;
		narrativeFlags: number;
		singlePlayerDaso?: any;
		singlePlayerDifficulty: number;
		coopDaso?: any;
		coopDifficulty: number;
		totalDuration: string;
		totalKills: number;
		totalDeaths: number;
		totalGamesStarted: number;
	}

	export interface SpartanOps {
		totalSinglePlayerMissionsCompleted: number;
		totalCoopMissionsCompleted: number;
		totalMissionsPossible: number;
		totalMedals: number;
		totalGamesWon: number;
		totalDuration: string;
		totalKills: number;
		totalDeaths: number;
		totalGamesStarted: number;
	}

	export interface WarGames {
		totalMedals: number;
		totalGamesWon: number;
		totalGamesCompleted: number;
		averagePersonalScore: number;
		kdRatio: number;
		totalGameBaseVariantMedals: number;
		favoriteVariant: FavoriteVariant;
		totalDuration: string;
		totalKills: number;
		totalDeaths: number;
		totalGamesStarted: number;
	}

	export interface FavoriteVariant {
		imageUrl: string;
		totalDuration: string;
		totalGamesStarted: number;
		totalGamesCompleted: number;
		totalGamesWon: number;
		totalMedals: number;
		totalKills: number;
		totalDeaths: number;
		kdRatio: number;
		averagePersonalScore: number;
		id: number;
		name: string;
	}
}
