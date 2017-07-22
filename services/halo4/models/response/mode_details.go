package response

type ModeDetailsShared struct {
	Response `bson:",inline"`

	ArticleID                        string                          `json:"articleId" bson:"articleId"`
	TotalChallengesCompleted         int                             `json:"totalChallengesCompleted" bson:"totalChallengesCompleted"`
	TotalMedalsStats                 *[]ModeDetailsMedalStat         `json:"totalMedalsStats" bson:"totalMedalsStats"`
	TopMedalIds                      []int                           `json:"topMedalIds" bson:"topMedalIds"`
	TotalAssists                     int                             `json:"totalAssists" bson:"totalAssists"`
	TotalHeadshots                   int                             `json:"totalHeadshots" bson:"totalHeadshots"`
	TotalKillsByDamageType           *[]ModeDetailsEventByDamageType `json:"totalKillsByDamageType" bson:"totalKillsByDamageType"`
	TotalDeathsByDamageType          *[]ModeDetailsEventByDamageType `json:"totalDeathsByDamageType" bson:"totalDeathsByDamageType"`
	TotalHeadshotsByWeapon           *[]ModeDetailsEventByDamageType `json:"totalHeadshotsByWeapon" bson:"totalHeadshotsByWeapon"`
	BestGameTotalKills               int                             `json:"bestGameTotalKills" bson:"bestGameTotalKills"`
	BestGameTotalKillsGameID         string                          `json:"bestGameTotalKillsGameId" bson:"bestGameTotalKillsGameId"`
	BestGameTotalMedals              int                             `json:"bestGameTotalMedals" bson:"bestGameTotalMedals"`
	BestGameTotalMedalsGameID        string                          `json:"bestGameTotalMedalsGameId" bson:"bestGameTotalMedalsGameId"`
	BestGameMedalsByTier             *[]ModeDetailsMedalByTier       `json:"bestGameMedalsByTier" bson:"bestGameMedalsByTier"`
	BestGameHeadshotTotal            int                             `json:"bestGameHeadshotTotal" bson:"bestGameHeadshotTotal"`
	BestGameHeadshotTotalGameID      string                          `json:"bestGameHeadshotTotalGameId" bson:"bestGameHeadshotTotalGameId"`
	BestGameAssassinationTotal       int                             `json:"bestGameAssassinationTotal" bson:"bestGameAssassinationTotal"`
	BestGameAssassinationTotalGameID string                          `json:"bestGameAssassinationTotalGameId" bson:"bestGameAssassinationTotalGameId"`
	BestGameKillDistance             int                             `json:"bestGameKillDistance" bson:"bestGameKillDistance"`
	BestGameKillDistanceGameID       string                          `json:"bestGameKillDistanceGameId" bson:"bestGameKillDistanceGameId"`
	BestDayTotalGames                int                             `json:"bestDayTotalGames" bson:"bestDayTotalGames"`
	BestDayTotalGamesDay             string                          `json:"bestDayTotalGamesDay" bson:"bestDayTotalGamesDay"`
	BestDayDuration                  string                          `json:"bestDayDuration" bson:"bestDayDuration"`
	BestDayDurationDay               string                          `json:"bestDayDurationDay" bson:"bestDayDurationDay"`
	BestDayTotalKills                int                             `json:"bestDayTotalKills" bson:"bestDayTotalKills"`
	BestDayTotalKillsDay             string                          `json:"bestDayTotalKillsDay" bson:"bestDayTotalKillsDay"`
	BestDayTotalMedals               int                             `json:"bestDayTotalMedals" bson:"bestDayTotalMedals"`
	BestDayTotalMedalsDay            string                          `json:"bestDayTotalMedalsDay" bson:"bestDayTotalMedalsDay"`
	BestDayMedalsByClass             *[]ModeDetailsMedalByClass      `json:"bestDayMedalsByClass" bson:"bestDayMedalsByClass"`
	BestDayMedalsByTier              *[]ModeDetailsMedalByTier       `json:"bestDayMedalsByTier" bson:"bestDayMedalsByTier"`
	BestDayHeadshotTotal             int                             `json:"bestDayHeadshotTotal" bson:"bestDayHeadshotTotal"`
	BestDayHeadshotTotalDay          string                          `json:"bestDayHeadshotTotalDay" bson:"bestDayHeadshotTotalDay"`
	BestDayAssassinationTotal        int                             `json:"bestDayAssassinationTotal" bson:"bestDayAssassinationTotal"`
	BestDayAssassinationTotalDay     string                          `json:"bestDayAssassinationTotalDay" bson:"bestDayAssassinationTotalDay"`
}

type ModeDetailsMedalStat struct {
	ID          int `json:"id" bson:"id"`
	TotalMedals int `json:"totalMedals" bson:"totalMedals"`
}

type ModeDetailsEventByDamageType struct {
	DamageTypeID int `json:"damageTypeId" bson:"damageTypeId"`
	Total        int `json:"total" bson:"total"`
}

type ModeDetailsMedalByClass struct {
	ClassID   int    `json:"classId" bson:"classId"`
	BestTotal int    `json:"bestTotal" bson:"bestTotal"`
	Day       string `json:"day" bson:"day"`
}

type ModeDetailsMedalByTier struct {
	TierID    int    `json:"tierId" bson:"tierId"`
	BestTotal int    `json:"bestTotal" bson:"bestTotal"`
	DayOrWeek string `json:"dayOrWeek" bson:"dayOrWeek"`
}

type ModeDetailsCompletionDifficulty struct {
	Difficulty       int `json:"difficulty" bson:"difficulty"`
	TotalCompletions int `json:"totalCompletions" bson:"totalCompletions"`
}

type ModeDetailsBeatDifficulty struct {
	Difficulty int `json:"difficulty" bson:"difficulty"`
	TotalBeat  int `json:"totalBeat" bson:"totalBeat"`
}

type ModeDetailsEnemyEvent struct {
	EnemyID int `json:"enemyId" bson:"enemyId"`
	Total   int `json:"total" bson:"total"`
}

type ModeDetailsVisitedTerminal struct {
	ID        int  `json:"id" bson:"id"`
	IsVisited bool `json:"isVisited" bson:"isVisited"`
}

type ModeDetailsBestCompletionDuration struct {
	Difficulty int    `json:"difficulty" bson:"difficulty"`
	Duration   string `json:"duration" bson:"duration"`
	GameID     string `json:"gameId" bson:"gameId"`
}
