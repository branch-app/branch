package halo4

// StatusCode is an enum representing Halo Waypoint response successfulness.
type StatusCode int

// GameMode is an enum representing Halo 4 Game Modes.
type GameMode uint

// MatchResult is an enum representing Halo 4 Match Results.
type MatchResult int

// DifficultyLevel is an enum representing Halo 4 campaign difficulties.
type DifficultyLevel uint

const (
	StatusCodeSuccess         StatusCode = 1
	StatusCodeContentNotFound StatusCode = 3
	StatusCodeNoData          StatusCode = 4
)

const (
	MatchResultUnknown MatchResult = -1
	MatchResultLost    MatchResult = 0
	MatchResultDraw    MatchResult = 1
	MatchResultWon     MatchResult = 2
)

const (
	WarGamesGameMode        GameMode = 3
	CampaignGameMode        GameMode = 4
	SpartanOpsGameMode      GameMode = 5
	WarGamesCustomsGameMode GameMode = 6
)

const (
	DifficultyEasy      DifficultyLevel = 0
	DifficultyNormal    DifficultyLevel = 1
	DifficultyHeroic    DifficultyLevel = 2
	DifficultyLegendary DifficultyLevel = 3
)
