package response

type ModeDetailsWarGames struct {
	ModeDetailsShared `bson:",inline"`

	Summary GameModeWarGames `json:"summary" bson:"summary"`

	TotalGames1StPlace int                                  `json:"totalGames1stPlace" bson:"totalGames1stPlace"`
	TotalGamesTopHalf  int                                  `json:"totalGamesTopHalf" bson:"totalGamesTopHalf"`
	TotalGamesTopThird int                                  `json:"totalGamesTopThird" bson:"totalGamesTopThird"`
	TotalBetrayals     int                                  `json:"totalBetrayals" bson:"totalBetrayals"`
	TotalSuicides      int                                  `json:"totalSuicides" bson:"totalSuicides"`
	GameBaseVariants   []ModeDetailsWarGamesGameBaseVariant `json:"gameBaseVariants" bson:"gameBaseVariants"`
	Maps               []ModeDetailsWarGamesMap             `json:"maps" bson:"maps"`
	SkillRanks         []SkillRank                          `json:"skillRanks" bson:"skillRanks"`
}

type ModeDetailsWarGamesGameBaseVariant struct {
	TotalGames1StPlace      int                            `json:"totalGames1stPlace" bson:"totalGames1stPlace"`
	TotalGamesTopHalf       int                            `json:"totalGamesTopHalf" bson:"totalGamesTopHalf"`
	TotalGamesTopThird      int                            `json:"totalGamesTopThird" bson:"totalGamesTopThird"`
	GameTypeMedalName       string                         `json:"gameTypeMedalName" bson:"gameTypeMedalName"`
	TotalGameTypeMedals     int                            `json:"totalGameTypeMedals" bson:"totalGameTypeMedals"`
	TotalAssists            int                            `json:"totalAssists" bson:"totalAssists"`
	TotalHeadshots          int                            `json:"totalHeadshots" bson:"totalHeadshots"`
	TotalBetrayals          int                            `json:"totalBetrayals" bson:"totalBetrayals"`
	TotalSuicides           int                            `json:"totalSuicides" bson:"totalSuicides"`
	TotalKillsByDamageType  []ModeDetailsEventByDamageType `json:"totalKillsByDamageType" bson:"totalKillsByDamageType"`
	TotalDeathsByDamageType []ModeDetailsEventByDamageType `json:"totalDeathsByDamageType" bson:"totalDeathsByDamageType"`
	TotalHeadshotsByWeapon  []ModeDetailsEventByDamageType `json:"totalHeadshotsByWeapon" bson:"totalHeadshotsByWeapon"`
	TotalMedalsStats        []ModeDetailsMedalStat         `json:"totalMedalsStats" bson:"totalMedalsStats"`
	BestPersonalScore       int                            `json:"bestPersonalScore" bson:"bestPersonalScore"`
	BestPersonalScoreGameID string                         `json:"bestPersonalScoreGameId" bson:"bestPersonalScoreGameId"`
	FeaturedStatName        string                         `json:"featuredStatName" bson:"featuredStatName"`
	BestFeaturedStatValue   int                            `json:"bestFeaturedStatValue" bson:"bestFeaturedStatValue"`
	BestFeaturedStatGameID  string                         `json:"bestFeaturedStatGameId" bson:"bestFeaturedStatGameId"`
	ImageURL                Asset                          `json:"imageUrl" bson:"imageUrl"`
	TotalDuration           string                         `json:"totalDuration" bson:"totalDuration"`
	TotalGamesStarted       int                            `json:"totalGamesStarted" bson:"totalGamesStarted"`
	TotalGamesCompleted     int                            `json:"totalGamesCompleted" bson:"totalGamesCompleted"`
	TotalGamesWon           int                            `json:"totalGamesWon" bson:"totalGamesWon"`
	TotalMedals             int                            `json:"totalMedals" bson:"totalMedals"`
	TotalKills              int                            `json:"totalKills" bson:"totalKills"`
	TotalDeaths             int                            `json:"totalDeaths" bson:"totalDeaths"`
	KDRatio                 float64                        `json:"kdRatio" bson:"kdRatio"`
	AveragePersonalScore    float64                        `json:"averagePersonalScore" bson:"averagePersonalScore"`
	ID                      int                            `json:"id" bson:"id"`
	Name                    string                         `json:"name" bson:"name"`
}

type ModeDetailsWarGamesMap struct {
	MapID                   int                            `json:"mapId" bson:"mapId"`
	Name                    string                         `json:"name" bson:"name"`
	Description             string                         `json:"description" bson:"description"`
	ImageURL                Asset                          `json:"imageUrl" bson:"imageUrl"`
	ArticleID               string                         `json:"articleId" bson:"articleId"`
	TotalGamesStarted       int                            `json:"totalGamesStarted" bson:"totalGamesStarted"`
	TotalGamesCompleted     int                            `json:"totalGamesCompleted" bson:"totalGamesCompleted"`
	TotalGamesWon           int                            `json:"totalGamesWon" bson:"totalGamesWon"`
	TotalGamesTied          int                            `json:"totalGamesTied" bson:"totalGamesTied"`
	TotalGamesLost          int                            `json:"totalGamesLost" bson:"totalGamesLost"`
	TotalKills              int                            `json:"totalKills" bson:"totalKills"`
	TotalDeaths             int                            `json:"totalDeaths" bson:"totalDeaths"`
	TotalAssists            int                            `json:"totalAssists" bson:"totalAssists"`
	TotalMedals             int                            `json:"totalMedals" bson:"totalMedals"`
	TotalMedalsStats        []ModeDetailsMedalStat         `json:"totalMedalsStats" bson:"totalMedalsStats"`
	TotalKillsByDamageType  []ModeDetailsEventByDamageType `json:"totalKillsByDamageType" bson:"totalKillsByDamageType"`
	TotalDeathsByDamageType []ModeDetailsEventByDamageType `json:"totalDeathsByDamageType" bson:"totalDeathsByDamageType"`
	TotalHeadshotsByWeapon  []ModeDetailsEventByDamageType `json:"totalHeadshotsByWeapon" bson:"totalHeadshotsByWeapon"`
}
