namespace Common {
	export interface SkillRank {
		currentSkillRank?: number;
		playlist: SkillRankPlaylist;
	}

	export interface SkillRankPlaylist {
		id: number;
		name: string;
		description: string;
		imageUrl: string;
	}
}
