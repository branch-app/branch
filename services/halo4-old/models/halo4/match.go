package halo4

import (
	"encoding/json"
	"fmt"
	"time"

	sharedHelpers "github.com/branch-app/shared-go/helpers"
	sharedModels "github.com/branch-app/shared-go/models"
	mgo "gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

// Match defines the structure of the Match response.
type Match struct {
	Response `bson:",inline"`

	// Game is the details of the match.
	Game MatchBase `json:"game" bson:"game"`
}

const MatchesCollectionName = "matches"

func (match *Match) Upsert(db *mgo.Database, url string, validFor time.Duration) error {
	now := time.Now().UTC()

	// Check if we need to set CreatedAt and ID
	if !match.BranchResponse.ID.Valid() {
		match.BranchResponse.ID = bson.NewObjectId()
		match.BranchResponse.CreatedAt = now
	}

	match.BranchResponse.UpdatedAt = now
	match.BranchResponse.CacheInformation = sharedModels.NewCacheInformation(url, now, validFor)
	_, err := db.C(MatchesCollectionName).UpsertId(match.BranchResponse.ID, match)
	return err
}

func MatchFindOne(db *mgo.Database, id bson.ObjectId) *Match {
	var document *Match
	err := db.C(MatchesCollectionName).FindId(id).One(&document)
	switch {
	case err == nil:
		return document
	case err.Error() == "not found":
		return nil
	default:
		panic(err)
	}
}

// MatchBase defines the custom base structure of a matche.
type MatchBase struct {
	ModeID  GameMode    `json:"modeId" bson:"modeId"`
	Details interface{} `json:"details" bson:"details"`
}

// MatchShared defines the common variables that are used in every MatchX.
type MatchShared struct {
	// Duration is the length of the time the match lasted.
	Duration string `json:"duration" bson:"duration"`

	// TotalPlayers is the number of players that were present at any time during the
	// match.
	TotalPlayers int `json:"totalPlayers" bson:"totalPlayers"`

	// Players is a list of each player present in the match.
	Players []MatchPlayer `json:"players" bson:"players"`

	// Teams is a list of each team present in the match.
	Teams []MatchTeam `json:"teams" bson:"teams"`

	// MapID is the ID of the map the match was played on.
	MapID int `json:"mapId" bson:"mapId"`

	// MapName is the name of the map the match was played on.
	MapName string `json:"mapName" bson:"mapName"`

	// MapImageURL is the asset of the map the match was played on.
	MapImageURL Asset `json:"mapImageUrl" bson:"mapImageUrl"`

	// ID is the identifier of the match.
	ID string `json:"id" bson:"id"`

	// ModeName is the name of the game mode in the match.
	ModeName string `json:"modeName" bson:"modeName"`

	// Completed is a bool declaring if the player stayed in the match until it
	// terminated.
	Completed bool `json:"completed" bson:"completed"`

	// Result is the completion state of the match.
	Result MatchResult `json:"result" bson:"result"`

	// EndDateUTC is the identifier of the match.
	EndDateUTC time.Time `json:"endDateUtc" bson:"endDateUtc"`

	// AverageDeathDistance holds the average death distance of the player.
	AverageDeathDistance float64 `json:"averageDeathDistance" bson:"averageDeathDistance"`

	// AverageKillDistance holds the average kill distance of the player.
	AverageKillDistance float64 `json:"averageKillDistance" bson:"averageKillDistance"`

	// TotalMedals holds the total number of medals earned by this player in the match.
	TotalMedals int `json:"totalMedals" bson:"totalMedals"`

	// TotalKillMedals holds the total number of kill medals earned by this player in the
	// match.
	TotalKillMedals int `json:"totalKillMedals" bson:"totalKillMedals"`

	// TotalBonusMedals holds the total number of bonus medals earned by this player in
	// the match.
	TotalBonusMedals int `json:"totalBonusMedals" bson:"totalBonusMedals"`

	// TotalAssistMedals holds the total number of assist medals earned by this player in
	// the match.
	TotalAssistMedals int `json:"totalAssistMedals" bson:"totalAssistMedals"`

	// TotalSpreeMedals holds the total number of spree medals earned by this player in
	// the match.
	TotalSpreeMedals int `json:"totalSpreeMedals" bson:"totalSpreeMedals"`

	// TotalModeMedals holds the total number of mode medals earned by this player in the
	// match.
	TotalModeMedals int `json:"totalModeMedals" bson:"totalModeMedals"`

	// TopMedalIDs holds a list of the top medals earned by the player in the match.
	TopMedalIDs []int `json:"topMedalIds" bson:"topMedalIds"`

	// Gamertag is the gamertag of the player at the time of the match.
	Gamertag string `json:"gamertag" bson:"gamertag"`
}

// MatchWarGames defines the structure of War Games matches.
type MatchWarGames struct {
	MatchShared `bson:",inline"`

	// MapVariantName is the variant of the map the match was played on.
	MapVariantName string `json:"mapVariantName" bson:"mapVariantName"`

	// PlaylistID is the ID of the playlist the match was played in.
	PlaylistID int `json:"playlistId" bson:"playlistId"`

	// PlaylistName is the name of the playlist the match was played in.
	PlaylistName string `json:"playlistName" bson:"playlistName"`

	// GameBaseVariantID is the id of the base game variant of the match.
	GameBaseVariantID int `json:"gameBaseVariantId" bson:"gameBaseVariantId"`

	// GameBaseVariantName is the name of the base game variant of the match.
	GameBaseVariantName string `json:"gameBaseVariantName" bson:"gameBaseVariantName"`

	// GameVariantName is the name of the game variant of the match.
	GameVariantName string `json:"gameVariantName" bson:"gameVariantName"`
}

// MatchCampaign defines the structure of Campaign matches.
type MatchCampaign struct {
	MatchShared `bson:",inline"`

	// Mission is the identifier of the mission this match was played on.
	Mission int `json:"mission" bson:"mission"`

	// Difficulty is the identifier of the difficulty this match was played on.
	Difficulty DifficultyLevel `json:"difficulty" bson:"difficulty"`

	// SkullIDs is the identifier of the mission this match was played on.
	SkullIDs []int `json:"skullIds" bson:"skullIds"`

	// CampaignScoringEnabled indicates if match scoring was enabled for this match.
	CampaignScoringEnabled bool `json:"campaignScoringEnabled" bson:"campaignScoringEnabled"`

	// CampaignGlobalScore hold the score if scoring was turned on.
	CampaignGlobalScore int `json:"campaignGlobalScore" bson:"campaignGlobalScore"`
}

// MatchSpartanOps defines the structure of Spartan Ops matches.
type MatchSpartanOps struct {
	MatchShared `bson:",inline"`

	// ChapterID is the identifier of the chapter this match was played on.
	ChapterID int `json:"chapterId" bson:"chapterId"`

	// Difficulty is the identifier of the difficulty this match was played on.
	Difficulty DifficultyLevel `json:"difficulty" bson:"difficulty"`

	// SinglePlayer indicates if the match was played on Single Player or Co-Op.
	SinglePlayer bool `json:"singlePlayer" bson:"singlePlayer"`
}

// MatchTeam defines the structure of a matches Team.
type MatchTeam struct {
	// ID is the identifier of the team.
	ID int `json:"id" bson:"id"`

	// Name is the friendly identifier of the team.
	Name string `json:"name" bson:"name"`

	// EmblemImageURL is the asset of the visual identifier of the team.
	EmblemImageURL string `json:"emblemImageUrl" bson:"emblemImageUrl"`

	// PrimaryRGB is the primary RGB representation of the team.
	PrimaryRGB string `json:"primaryRgb" bson:"primaryRgb"`

	// PrimaryRGBA is the primary RGBA representation of the team.
	PrimaryRGBA int64 `json:"primaryRgba" bson:"primaryRgba"`

	// SecondaryRGB is the secondary RGB representation of the team.
	SecondaryRGB string `json:"secondaryRgb" bson:"secondaryRgb"`

	// SecondaryRGBA is the secondary RGBA representation of the team.
	SecondaryRGBA int64 `json:"secondaryRgba" bson:"secondaryRgba"`

	// Standing holds the standing position of the team.
	Standing int `json:"standing" bson:"standing"`

	// Score holds the game score of the team.
	Score int `json:"score" bson:"score"`

	// Kills holds the number of kills made by the team.
	Kills int `json:"score" bson:"score"`

	// Deaths holds the number of deaths by the team.
	Deaths int `json:"deaths" bson:"deaths"`

	// Assists holds the number of assists made by the team.
	Assists int `json:"assists" bson:"assists"`

	// Betrayals holds the number of betrayals made by the team.
	Betrayals int `json:"betrayals" bson:"betrayals"`

	// Suicides holds the number of suicides made by the team.
	Suicides int `json:"suicides" bson:"suicides"`

	// DeathsOverTime holds the death over time stats for the team.
	DeathsOverTime []MatchStatOverTime `json:"deathsOverTime" bson:"deathsOverTime"`

	// KillsOverTime holds the kills over time stats for the team.
	KillsOverTime []MatchStatOverTime `json:"killsOverTime" bson:"killsOverTime"`

	// MedalsOverTime holds the medals over time stats for the team.
	MedalsOverTime []MatchStatOverTime `json:"medalsOverTime" bson:"medalsOverTime"`

	// MedalStats hold the stats for how many times medals have been earned by the team.
	MedalStats []MatchMedalStat `json:"medalStats" bson:"medalStats"`
}

// MatchPlayer defines the structure of a matches Player.
type MatchPlayer struct {
	// DeathsOverTime holds the death over time stats for the player.
	DeathsOverTime []MatchStatOverTime `json:"deathsOverTime" bson:"deathsOverTime"`

	// KillsOverTime holds the kills over time stats for the player.
	KillsOverTime []MatchStatOverTime `json:"killsOverTime" bson:"killsOverTime"`

	// MedalsOverTime holds the medals over time stats for the player.
	MedalsOverTime []MatchStatOverTime `json:"medalsOverTime" bson:"medalsOverTime"`

	// MedalStats holds the stats for how many times medals have been earned by the player.
	MedalStats []MatchMedalStat `json:"medalStats" bson:"medalStats"`

	// DamageTypeStats holds the damage type stats of the player.
	DamageTypeStats []MatchStatOverTime `json:"damageTypeStats" bson:"damageTypeStats"`

	// EnemyStats holds the enemy stats of the player.
	EnemyStats []MatchMedalStat `json:"enemyStats" bson:"enemyStats"`

	// SkillRank holds the skill rank of the player at the time of the match. This is
	// omitted from Campaign, Spartan Ops, and Custom Games.
	SkillRank *MatchPlayerSkillRank `json:"skillRank" bson:"skillRank"`

	// TeamID holds the identifier of the team this player belongs to. It will be -1 if
	// the player doesn't belong to a team.
	TeamID int16 `json:"teamId" bson:"teamId"`

	// IsCompleted indicates if the player stayed in the match until the end.
	IsCompleted bool `json:"isCompleted" bson:"isCompleted"`

	// ServiceTag holds the players Service Tag.
	ServiceTag string `json:"serviceTag" bson:"serviceTag"`

	// IsGuest defines if the player is a guest account.
	IsGuest bool `json:"isGuest" bson:"isGuest"`

	// JoinedInProgress indicates if the player joined after the match was already in
	// progress.
	JoinedInProgress bool `json:"joinedInProgress" bson:"joinedInProgress"`

	// Standing holds the players standing at the end of the match.
	Standing int `json:"standing" bson:"standing"`

	// Result indicates the result of the player at the end of the match.
	Result MatchResult `json:"result" bson:"result"`

	// PersonalScore holds the personal score of the player at the end of the match.
	PersonalScore int `json:"personalScore" bson:"personalScore"`

	// FeaturedStatName holds the name of the matches featured stat.
	FeaturedStatName string `json:"featuredStatName" bson:"featuredStatName"`

	// FeaturedStatValue holds the players value of the featured stat of the match.
	FeaturedStatValue int `json:"featuredStatValue" bson:"featuredStatValue"`

	// StandingInTeam holds the players standing in it's team. It will be -1 if the player
	// doesn't beling to a team.
	StandingInTeam int `json:"standingInTeam" bson:"standingInTeam"`

	// Kills holds the total number of kills for the player.
	Kills int `json:"kills" bson:"kills"`

	// Deaths holds the total number of deaths for the player.
	Deaths int `json:"deaths" bson:"deaths"`

	// Assists holds the total number of assists for the player.
	Assists int `json:"assists" bson:"assists"`

	// Headshots holds the total number of headshots for the player.
	Headshots int `json:"headshots" bson:"headshots"`

	// Betrayals holds the total number of betrayals for the player.
	Betrayals int `json:"betrayals" bson:"betrayals"`

	// Suicides holds the total number of suicides for the player.
	Suicides int `json:"suicides" bson:"suicides"`

	// KilledMostGamertag holds the gamertag of the player this player killed the most.
	KilledMostGamertag *string `json:"killedMostGamertag" bson:"killedMostGamertag"`

	// KilledMostCount holds the number of times this player killed the player they killed
	// the most in the match.
	KilledMostCount int `json:"killedMostCount" bson:"killedMostCount"`

	// KilledByMostGamertag holds the gamertag of the player this player was killed the
	// most by.
	KilledByMostGamertag *string `json:"killedByMostGamertag" bson:"killedByMostGamertag"`

	// KilledByMostCount holds the number of times this player was killed by the player
	// that killed them the most in the match.
	KilledByMostCount int `json:"killedByMostCount" bson:"killedByMostCount"`

	// RankID holds the identifier of the rank this player was at the time of the match.
	RankID int `json:"rankId" bson:"rankId"`

	// RankName holds the friendly identifier of the rank this player was at the time of
	// the match.
	RankName string `json:"rankName" bson:"rankName"`

	// RankImage holds the visual identifier of the rank this player was at the time of
	// the match.
	RankImage Asset `json:"rankImage" bson:"rankImage"`

	// EmblemImageURL holds the asset of the emblem of the player.
	EmblemImageURL Asset `json:"emblemImageUrl" bson:"emblemImageUrl"`

	// FirstPlayedUTC holds the time the player first played Halo 4.
	FirstPlayedUTC time.Time `json:"firstPlayedUtc" bson:"firstPlayedUtc"`

	// LastPlayedUTC holds the time the player last played Halo 4.
	LastPlayedUTC time.Time `json:"lastPlayedUtc" bson:"lastPlayedUtc"`

	// AverageKillDistance holds the average kill distance for this player.
	AverageKillDistance float64 `json:"averageKillDistance" bson:"averageKillDistance"`

	// AverageDeathDistance holds the average death distance for this player.
	AverageDeathDistance float64 `json:"averageDeathDistance" bson:"averageDeathDistance"`

	// TotalMedals holds the number of medals earned by this player in the match.
	TotalMedals int `json:"totalMedals" bson:"totalMedals"`

	// TotalKillMedals holds the number of kill medals earned by this player in the match.
	TotalKillMedals int `json:"totalKillMedals" bson:"totalKillMedals"`

	// TotalBonusMedals holds the number of bonus medals earned by this player in the
	// match.
	TotalBonusMedals int `json:"totalBonusMedals" bson:"totalBonusMedals"`

	// TotalAssistMedals holds the number of assist medals earned by this player in the
	// match.
	TotalAssistMedals int `json:"totalAssistMedals" bson:"totalAssistMedals"`

	// TotalSpreeMedals holds the number of spree medals earned by this player in the
	// match.
	TotalSpreeMedals int `json:"totalSpreeMedals" bson:"totalSpreeMedals"`

	// TotalModeMedals holds the number of mode medals earned by this player in the match.
	TotalModeMedals int `json:"totalModeMedals" bson:"totalModeMedals"`

	// TopMedalIDs holds the top medals earned by this player in the match.
	TopMedalIDs []int `json:"topMedalIds" bson:"topMedalIds"`

	// Gamertag holds the name of this player in the match.
	Gamertag string `json:"gamertag" bson:"gamertag"`
}

// MatchStatOverTime defines the structure of a OverTime statistic.
// - Time is the number of seconds into the match the event happened.
// - Ticks is the number of times the event elapsed.
type MatchStatOverTime struct {
	Time  int `json:"time" bson:"time"`
	Ticks int `json:"ticks" bson:"ticks"`
}

// MatchMedalStat defines the structure of the representation of a medal, and how many
// times it has been earned.
type MatchMedalStat struct {
	// ClassID holds the ID of the class the medal belongs to.
	ClassID int `json:"classId" bson:"classId"`

	// Name holds the friendly name of the medal.
	Name string `json:"name" bson:"name"`

	// ImageURL holds the asset representation of the medal.
	ImageURL Asset `json:"imageUrl" bson:"imageUrl"`

	// ID holds the identifier of the medal.
	ID int `json:"id" bson:"id"`

	// TotalMedals holds the total number of times the medal was earned.
	TotalMedals int `json:"totalMedals" bson:"totalMedals"`
}

// MatchDamageTypeStat defines the structure of a damage type statistic, and it's visual
// representation.
type MatchDamageTypeStat struct {
	// ID holds the identifier of the damage type.
	ID int `json:"id" bson:"id"`

	// Name holds the friendly name of the damage type.
	Name string `json:"name" bson:"name"`

	// ImageURL holds the asset representation of the damage type.
	ImageURL Asset `json:"imageUrl" bson:"imageUrl"`

	// Kills holds the number of kills made by this damage type.
	Kills int `json:"kills" bson:"kills"`

	// Deaths holds the number of deaths caused by this damage type.
	Deaths int `json:"deaths" bson:"deaths"`

	// Headshots holds the number of headshots made by this damage type.
	Headshots int `json:"headshots" bson:"headshots"`

	// Betrayals holds the number of betrayals made by this damage type.
	Betrayals int `json:"betrayals" bson:"betrayals"`

	// Suicides holds the number of suicides caused by this damage type.
	Suicides int `json:"suicides" bson:"suicides"`
}

// MatchEnemyStat defines the structure of a enemy statistic, and it's visual
// representation.
type MatchEnemyStat struct {
	// EnemyID holds the identifier of the enemy.
	EnemyID int `json:"enemyId" bson:"enemyId"`

	// Name holds the friendly name of the enemy.
	Name string `json:"name" bson:"name"`

	// ImageURL holds the asset representation of the enemy.
	ImageURL Asset `json:"imageUrl" bson:"imageUrl"`

	// Kills holds the number of kills of this type of enemy.
	Kills int `json:"kills" bson:"kills"`

	// Deaths holds the number of deaths of this type of enemy.
	Deaths int `json:"deaths" bson:"deaths"`

	// AverageKillDistance holds the average kill distance of this enemy.
	AverageKillDistance float64 `json:"averageKillDistance" bson:"averageKillDistance"`

	// AverageDeathDistance holds the average death distance of this enemy.
	AverageDeathDistance float64 `json:"averageDeathDistance" bson:"averageDeathDistance"`
}

// MatchPlayerSkillRank defines the structure of a players skill rank.
type MatchPlayerSkillRank struct {
	// CurrentSkillRank is the skill rank of the player at the start of the match. It will
	// be null if they don't have a CSR yet.
	CurrentSkillRank *int `json:"currentSkillRank" bson:"currentSkillRank"`

	// GameSkillRank is the skill rank of the player at the end of the match. It will be
	// null if they don't have a CSR yet.
	GameSkillRank *int `json:"gameSkillRank" bson:"gameSkillRank"`

	// PlaylistDescription is the description of this playlist.
	PlaylistDescription string `json:"playlistDescription" bson:"playlistDescription"`

	// PlaylistID is the description of this playlist.
	PlaylistID int `json:"playlistId" bson:"playlistId"`

	// PlaylistImageURL is the description of this playlist.
	PlaylistImageURL Asset `json:"playlistImageUrl" bson:"playlistImageUrl"`

	// PlaylistName is the description of this playlist.
	PlaylistName string `json:"playlistName" bson:"playlistName"`
}

// UnmarshalJSON unmarshals bullshit dynamic Game Mode JSON from the Service Record into
// easy to work with structs. Fuck you 343.
func (u *MatchBase) UnmarshalJSON(data []byte) error {
	type Alias struct {
		ModeID GameMode
	}

	// Unmarshal JSON into temp alias struct
	var alias Alias
	err := json.Unmarshal(data, &alias)
	if err != nil {
		return err
	}

	// Set alias data to GameModeBase
	u.ModeID = alias.ModeID

	switch u.ModeID {
	case WarGamesGameMode, WarGamesCustomsGameMode: // War Games || War Games - Customs
		var wargamesMatch MatchWarGames
		if err = json.Unmarshal(data, &wargamesMatch); err != nil {
			return err
		}
		u.Details = wargamesMatch
		break
	case CampaignGameMode: // Campaign
		var campaignMatch MatchCampaign
		if err = json.Unmarshal(data, &campaignMatch); err != nil {
			return err
		}
		u.Details = campaignMatch
		break
	case SpartanOpsGameMode: // Spartan Ops
		var spartanOpsMatch MatchSpartanOps
		if err = json.Unmarshal(data, &spartanOpsMatch); err != nil {
			return err
		}
		u.Details = spartanOpsMatch
		break
	}

	return nil
}

// MarshalJSON converts the custom data format back into the JSON format 343 uses >_>
func (u *MatchBase) MarshalJSON() ([]byte, error) {
	data, err := json.Marshal(u.Details)
	if err != nil {
		return data, err
	}

	// Create custom JSON string with Game Mode ID and Name
	customJSONFormat := `"modeId":%d,`
	customJSON := fmt.Sprintf(customJSONFormat, u.ModeID)
	return sharedHelpers.ByteSliceInsert(data, []byte(customJSON), 1)
}
