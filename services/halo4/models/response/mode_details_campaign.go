package response

type ModeDetailsCampaign struct {
	ModeDetailsShared `bson:",inline"`

	Summary GameModeCampaign `json:"summary" bson:"summary"`

	TotalGamesCompleted               int                          `json:"totalGamesCompleted" bson:"totalGamesCompleted"`
	TotalMapsBeatSinglePlayer         int                          `json:"totalMapsBeatSinglePlayer" bson:"totalMapsBeatSinglePlayer"`
	TotalMapsBeatCoop                 int                          `json:"totalMapsBeatCoop" bson:"totalMapsBeatCoop"`
	TotalMapsBeatSinglePlayerNoDeaths int                          `json:"totalMapsBeatSinglePlayerNoDeaths" bson:"totalMapsBeatSinglePlayerNoDeaths"`
	TotalMapsBeatCoopNoDeaths         int                          `json:"totalMapsBeatCoopNoDeaths" bson:"totalMapsBeatCoopNoDeaths"`
	TotalMissionsCompleted            int                          `json:"totalMissionsCompleted" bson:"totalMissionsCompleted"`
	TerminalsVisited                  []ModeDetailsVisitedTerminal `json:"terminalsVisited" bson:"terminalsVisited"`
	Maps                              []ModeDetailsCampaignMap     `json:"maps" bson:"maps"`
}

type ModeDetailsCampaignMap struct {
	Mission                                 int                               `json:"mission" bson:"mission"`
	SinglePlayerBeatByDifficulty            []ModeDetailsCompletionDifficulty `json:"singlePlayerBeatByDifficulty" bson:"singlePlayerBeatByDifficulty"`
	CoopBeatByDifficulty                    []ModeDetailsCompletionDifficulty `json:"coopBeatByDifficulty" bson:"coopBeatByDifficulty"`
	SinglePlayerBeatDASO                    []ModeDetailsCompletionDifficulty `json:"singlePlayerBeatDaso" bson:"singlePlayerBeatDaso"`
	CoopBeatDASO                            []ModeDetailsCompletionDifficulty `json:"coopBeatDaso" bson:"coopBeatDaso"`
	SinglePlayerBeatNoDeaths                []ModeDetailsCompletionDifficulty `json:"singlePlayerBeatNoDeaths" bson:"singlePlayerBeatNoDeaths"`
	CoopBeatNoDeaths                        []ModeDetailsCompletionDifficulty `json:"coopBeatNoDeaths" bson:"coopBeatNoDeaths"`
	TotalKillsOfEnemy                       []ModeDetailsEnemyEvent           `json:"totalKillsOfEnemy" bson:"totalKillsOfEnemy"`
	TotalDeathsByEnemy                      []ModeDetailsEnemyEvent           `json:"totalDeathsByEnemy" bson:"totalDeathsByEnemy"`
	BestGameCompletionDuration              string                            `json:"bestGameCompletionDuration" bson:"bestGameCompletionDuration"`
	BestGameCompletionDurationGameID        string                            `json:"bestGameCompletionDurationGameId" bson:"bestGameCompletionDurationGameId"`
	BestSinglePlayerDurationLegendary       string                            `json:"bestSinglePlayerDurationLegendary" bson:"bestSinglePlayerDurationLegendary"`
	BestSinglePlayerDurationLegendaryGameID string                            `json:"bestSinglePlayerDurationLegendaryGameId" bson:"bestSinglePlayerDurationLegendaryGameId"`
	BestSinglePlayerDurationHeroic          string                            `json:"bestSinglePlayerDurationHeroic" bson:"bestSinglePlayerDurationHeroic"`
	BestSinglePlayerDurationHeroicGameID    string                            `json:"bestSinglePlayerDurationHeroicGameId" bson:"bestSinglePlayerDurationHeroicGameId"`
	BestSinglePlayerDurationNormal          string                            `json:"bestSinglePlayerDurationNormal" bson:"bestSinglePlayerDurationNormal"`
	BestSinglePlayerDurationNormalGameID    string                            `json:"bestSinglePlayerDurationNormalGameId" bson:"bestSinglePlayerDurationNormalGameId"`
	BestSinglePlayerDurationEasy            string                            `json:"bestSinglePlayerDurationEasy" bson:"bestSinglePlayerDurationEasy"`
	BestSinglePlayerDurationEasyGameID      string                            `json:"bestSinglePlayerDurationEasyGameId" bson:"bestSinglePlayerDurationEasyGameId"`
	BestCoopDurationLegendary               string                            `json:"bestCoopDurationLegendary" bson:"bestCoopDurationLegendary"`
	BestCoopDurationLegendaryGameID         string                            `json:"bestCoopDurationLegendaryGameId" bson:"bestCoopDurationLegendaryGameId"`
	BestCoopDurationHeroic                  string                            `json:"bestCoopDurationHeroic" bson:"bestCoopDurationHeroic"`
	BestCoopDurationHeroicGameID            string                            `json:"bestCoopDurationHeroicGameId" bson:"bestCoopDurationHeroicGameId"`
	BestCoopDurationNormal                  string                            `json:"bestCoopDurationNormal" bson:"bestCoopDurationNormal"`
	BestCoopDurationNormalGameID            string                            `json:"bestCoopDurationNormalGameId" bson:"bestCoopDurationNormalGameId"`
	BestCoopDurationEasy                    string                            `json:"bestCoopDurationEasy" bson:"bestCoopDurationEasy"`
	BestCoopDurationEasyGameID              string                            `json:"bestCoopDurationEasyGameId" bson:"bestCoopDurationEasyGameId"`
	BestSinglePlayerScore                   int                               `json:"bestSinglePlayerScore" bson:"bestSinglePlayerScore"`
	BestSinglePlayerScoreGameID             string                            `json:"bestSinglePlayerScoreGameId" bson:"bestSinglePlayerScoreGameId"`
	BestCoopScore                           int                               `json:"bestCoopScore" bson:"bestCoopScore"`
	BestCoopScoreGameID                     string                            `json:"bestCoopScoreGameId" bson:"bestCoopScoreGameId"`
	FirstCompletedSinglePlayer              string                            `json:"firstCompletedSinglePlayer" bson:"firstCompletedSinglePlayer"`
	FirstCompletedCoop                      string                            `json:"firstCompletedCoop" bson:"firstCompletedCoop"`
	TotalDuration                           string                            `json:"totalDuration" bson:"totalDuration"`
	MapID                                   int                               `json:"mapId" bson:"mapId"`
	Name                                    string                            `json:"name" bson:"name"`
	Description                             string                            `json:"description" bson:"description"`
	ImageURL                                Asset                             `json:"imageUrl" bson:"imageUrl"`
	ArticleID                               string                            `json:"articleId" bson:"articleId"`
	TotalGamesStarted                       int                               `json:"totalGamesStarted" bson:"totalGamesStarted"`
	TotalGamesCompleted                     int                               `json:"totalGamesCompleted" bson:"totalGamesCompleted"`
	TotalGamesWon                           int                               `json:"totalGamesWon" bson:"totalGamesWon"`
	TotalGamesTied                          int                               `json:"totalGamesTied" bson:"totalGamesTied"`
	TotalGamesLost                          int                               `json:"totalGamesLost" bson:"totalGamesLost"`
	TotalKills                              int                               `json:"totalKills" bson:"totalKills"`
	TotalDeaths                             int                               `json:"totalDeaths" bson:"totalDeaths"`
	TotalAssists                            int                               `json:"totalAssists" bson:"totalAssists"`
	TotalMedals                             int                               `json:"totalMedals" bson:"totalMedals"`
	TotalMedalsStats                        *[]ModeDetailsMedalStat           `json:"totalMedalsStats" bson:"totalMedalsStats"`
	TotalKillsByDamageType                  []ModeDetailsEventByDamageType    `json:"totalKillsByDamageType" bson:"totalKillsByDamageType"`
	TotalDeathsByDamageType                 []ModeDetailsEventByDamageType    `json:"totalDeathsByDamageType" bson:"totalDeathsByDamageType"`
	TotalHeadshotsByWeapon                  []ModeDetailsEventByDamageType    `json:"totalHeadshotsByWeapon" bson:"totalHeadshotsByWeapon"`
}
