package response

type ModeDetailsSpartanOps struct {
	ModeDetailsShared `bson:",inline"`

	Summary GameModeSpartanOps `json:"summary" bson:"summary"`

	TotalGamesCompleted                   int                          `json:"totalGamesCompleted" bson:"totalGamesCompleted"`
	TotalChaptersBeatSinglePlayer         int                          `json:"totalChaptersBeatSinglePlayer" bson:"totalChaptersBeatSinglePlayer"`
	TotalChaptersBeatCoop                 int                          `json:"totalChaptersBeatCoop" bson:"totalChaptersBeatCoop"`
	TotalChaptersBeatSinglePlayerNoDeaths int                          `json:"totalChaptersBeatSinglePlayerNoDeaths" bson:"totalChaptersBeatSinglePlayerNoDeaths"`
	TotalChaptersBeatCoopNoDeaths         int                          `json:"totalChaptersBeatCoopNoDeaths" bson:"totalChaptersBeatCoopNoDeaths"`
	BestDayTotalWins                      int                          `json:"bestDayTotalWins" bson:"bestDayTotalWins"`
	BestDayTotalWinsDay                   string                       `json:"bestDayTotalWinsDay" bson:"bestDayTotalWinsDay"`
	UniqueChaptersCompleted               int                          `json:"uniqueChaptersCompleted" bson:"uniqueChaptersCompleted"`
	TotalChaptersAvailable                int                          `json:"totalChaptersAvailable" bson:"totalChaptersAvailable"`
	SeasonsReleasedToDate                 int                          `json:"seasonsReleasedToDate" bson:"seasonsReleasedToDate"`
	Seasons                               []ModeDetailSpartanOpsSeason `json:"seasons" bson:"seasons"`
}

type ModeDetailSpartanOpsSeason struct {
	ID              int                            `json:"id" bson:"id"`
	IsCurrentSeason bool                           `json:"isCurrentSeason" bson:"isCurrentSeason"`
	CurrentEpisode  int                            `json:"currentEpisode" bson:"currentEpisode"`
	Episodes        []ModeDetailsSpartanOpsEpisode `json:"episodes" bson:"episodes"`
}

type ModeDetailsSpartanOpsEpisode struct {
	ID          int                            `json:"id" bson:"id"`
	Title       string                         `json:"title" bson:"title"`
	Description string                         `json:"description" bson:"description"`
	ImageURL    Asset                          `json:"imageUrl" bson:"imageUrl"`
	Chapters    []ModeDetailsSpartanOpsChapter `json:"chapters" bson:"chapters"`
}

type ModeDetailsSpartanOpsChapter struct {
	ID                         int                                 `json:"id" bson:"id"`
	Number                     int                                 `json:"number" bson:"number"`
	ImageURL                   Asset                               `json:"imageUrl" bson:"imageUrl"`
	TotalGamesStarted          int                                 `json:"totalGamesStarted" bson:"totalGamesStarted"`
	TotalGamesCompleted        int                                 `json:"totalGamesCompleted" bson:"totalGamesCompleted"`
	TotalDuration              string                              `json:"totalDuration" bson:"totalDuration"`
	TotalMedals                int                                 `json:"totalMedals" bson:"totalMedals"`
	TotalMedalsStats           []ModeDetailsMedalStat              `json:"totalMedalsStats" bson:"totalMedalsStats"`
	TotalKills                 int                                 `json:"totalKills" bson:"totalKills"`
	TotalDeaths                int                                 `json:"totalDeaths" bson:"totalDeaths"`
	TotalAssists               int                                 `json:"totalAssists" bson:"totalAssists"`
	TotalKillsByDamageType     []ModeDetailsEventByDamageType      `json:"totalKillsByDamageType" bson:"totalKillsByDamageType"`
	TotalDeathsByDamageType    []ModeDetailsEventByDamageType      `json:"totalDeathsByDamageType" bson:"totalDeathsByDamageType"`
	TotalHeadshotsByWeapon     []ModeDetailsEventByDamageType      `json:"totalHeadshotsByWeapon" bson:"totalHeadshotsByWeapon"`
	TotalKillsOfEnemy          []ModeDetailsEnemyEvent             `json:"totalKillsOfEnemy" bson:"totalKillsOfEnemy"`
	TotalDeathsByEnemy         []ModeDetailsEnemyEvent             `json:"totalDeathsByEnemy" bson:"totalDeathsByEnemy"`
	SinglePlayerBeat           []ModeDetailsBeatDifficulty         `json:"singlePlayerBeat" bson:"singlePlayerBeat"`
	CoopBeat                   []ModeDetailsBeatDifficulty         `json:"coopBeat" bson:"coopBeat"`
	SinglePlayerBeatNoDeaths   []ModeDetailsBeatDifficulty         `json:"singlePlayerBeatNoDeaths" bson:"singlePlayerBeatNoDeaths"`
	CoopBeatNoDeaths           []ModeDetailsBeatDifficulty         `json:"coopBeatNoDeaths" bson:"coopBeatNoDeaths"`
	BestGameDuration           string                              `json:"bestGameDuration" bson:"bestGameDuration"`
	BestGameDurationGameID     string                              `json:"bestGameDurationGameId" bson:"bestGameDurationGameId"`
	FirstCompletedSinglePlayer string                              `json:"firstCompletedSinglePlayer" bson:"firstCompletedSinglePlayer"`
	FirstCompletedCoop         string                              `json:"firstCompletedCoop" bson:"firstCompletedCoop"`
	BestSinglePlayerDurations  []ModeDetailsBestCompletionDuration `json:"bestSinglePlayerDurations" bson:"bestSinglePlayerDurations"`
	BestCoopDurations          []ModeDetailsBestCompletionDuration `json:"bestCoopDurations" bson:"bestCoopDurations"`
}
