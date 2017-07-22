package response

import (
	"encoding/json"
	"fmt"

	"github.com/branch-app/branch-mono-go/helpers"
)

// GameModeBase defines the custom base structure of the Service Record GameModes. It has
// custom un-marshaling code for dealing with annoying dynamic JSON.
type GameModeBase struct {
	ID      GameMode    `json:"id" bson:"id"`
	Name    string      `json:"name" bson:"name"`
	Details interface{} `json:"details" bson:"details"`
}

// GameModeShared defines the common variables that are used in every GameModeX.
type GameModeShared struct {
	// TotalDuration is the total amount of time the player has spent in this game mode.
	TotalDuration string `json:"totalDuration" bson:"totalDuration"`

	// TotalKills is the total number of kills the player has earned in this game mode.
	TotalKills uint `json:"totalKills" bson:"totalKills"`

	// TotalDeaths is the total number of deaths the player has had in this game mode.
	TotalDeaths uint `json:"totalDeaths" bson:"totalDeaths"`

	// TotalGamesStarted is the total number of games started by this player.
	TotalGamesStarted uint `json:"totalGamesStarted" bson:"totalGamesStarted"`
}

// GameModeWarGames defines the structure of War Games leaned GameMode details about a
// player.
type GameModeWarGames struct {
	GameModeShared `bson:",inline"`

	// TotalGamesCompleted is the total number of games the player has completed.
	TotalGamesCompleted uint `json:"totalGamesCompleted" bson:"totalGamesCompleted"`

	// TotalGamesWon is the total number of games won by the player.
	TotalGamesWon uint `json:"totalGamesWon" bson:"totalGamesWon"`

	// TotalMedals is the total number of medals earned by the player.
	TotalMedals uint `json:"totalMedals" bson:"totalMedals"`

	// AveragePersonalScore is the average score the player has earned in this game mode.
	AveragePersonalScore uint `json:"averagePersonalScore" bson:"averagePersonalScore"`

	// KDRatio is the kill/death ratio of the player in this game mode.
	KDRatio float64 `json:"kdRatio" bson:"kdRatio"`

	// TotalGameBaseVariantMedals is the total number of game base variant medals
	TotalGameBaseVariantMedals uint `json:"totalGameBaseVariantMedals" bson:"totalGameBaseVariantMedals"`

	// FavoriteVariant
	FavoriteVariant WarGamesVariant `json:"favoriteVariant" bson:"favoriteVariant"`
}

// WarGamesVariant is a game mode variant based on War Games.
type WarGamesVariant struct {
	GameModeShared `bson:",inline"`

	// ID is the identifier of the Game Mode Variant.
	ID uint `json:"id" bson:"id"`

	// Name is the name of the Game Mode Variant.
	Name string `json:"name" bson:"name"`

	// ImageURL is the asset of the Game Mode Variant.
	ImageURL *Asset `json:"imageUrl" bson:"imageUrl"`
}

// GameModeCampaign defines the structure of Campaign leaned GameMode details about a
// player.
type GameModeCampaign struct {
	GameModeShared `bson:",inline"`

	// DifficultyLevels is a list of each of the Campaigns Difficulty Levels.
	DifficultyLevels []Difficulty `json:"difficultyLevels" bson:"difficultyLevels"`

	// SinglePlayerMissions is the mission completion state of each mission played in
	// Single Player.
	SinglePlayerMissions []MissionCompletion `json:"singlePlayerMissions" bson:"singlePlayerMissions"`

	// CoopMissions is the mission completion state of each mission played in Coop.
	CoopMissions []MissionCompletion `json:"coopMissions" bson:"coopMissions"`

	// TotalTerminalsVisited is the total number of terminals visited by the player.
	TotalTerminalsVisited uint `json:"totalTerminalsVisited" bson:"totalTerminalsVisited"`

	// NarrativeFlags - I have literally no idea what this is.
	NarrativeFlags uint `json:"narrativeFlags" bson:"narrativeFlags"`

	// SinglePlayerDASO is the highest difficulty the player has completed the game on
	// while playing Single Player with All Skulls On.
	SinglePlayerDASO *DifficultyLevel `json:"singlePlayerDASO" bson:"singlePlayerDASO"`

	// SinglePlayerDifficulty is the highest difficulty the player has completed the game on
	// while playing Single Player.
	SinglePlayerDifficulty *DifficultyLevel `json:"singlePlayerDifficulty" bson:"singlePlayerDifficulty"`

	// CoopDASO is the highest difficulty the player has completed the game on
	// while playing Coop with All Skulls On.
	CoopDASO *DifficultyLevel `json:"coopDASO" bson:"coopDASO"`

	// SinglePlayerDifficulty is the highest difficulty the player has completed the game on
	// while playing Coop.
	CoopDifficulty *DifficultyLevel `json:"coopDifficulty" bson:"coopDifficulty"`
}

// GameModeSpartanOps defines the structure of Spartan Ops leaned GameMode details about a
// player.
type GameModeSpartanOps struct {
	GameModeShared `bson:",inline"`

	// TotalSinglePlayerMissionsCompleted is the total number of Single Player Spartan Ops
	// missions started by the player.
	TotalSinglePlayerMissionsCompleted uint `json:"totalSinglePlayerMissionsCompleted" bson:"totalSinglePlayerMissionsCompleted"`

	// TotalCoopMissionsCompleted is the total number of Coop Spartan Ops missions started
	// by the player.
	TotalCoopMissionsCompleted uint `json:"totalCoopMissionsCompleted" bson:"totalCoopMissionsCompleted"`

	// TotalMissionsPossible - I have no idea what this is.
	TotalMissionsPossible uint `json:"totalMissionsPossible" bson:"totalMissionsPossible"`

	// TotalMedals is the total number of medals earned by the player.
	TotalMedals uint `json:"totalMedals" bson:"totalMedals"`
}

// UnmarshalJSON unmarshals bullshit dynamic Game Mode JSON from the Service Record into
// easy to work with structs. Fuck you 343.
func (u *GameModeBase) UnmarshalJSON(data []byte) error {
	type Alias struct {
		ID   GameMode
		Name string
	}

	// Unmarshal JSON into temp alias struct
	var alias Alias
	err := json.Unmarshal(data, &alias)
	if err != nil {
		return err
	}

	// Set alias data to GameModeBase
	u.ID = alias.ID
	u.Name = alias.Name

	switch u.ID {
	case WarGamesGameMode, WarGamesCustomsGameMode: // War Games || War Games - Customs
		var wargamesMode GameModeWarGames
		err = json.Unmarshal(data, &wargamesMode)
		if err != nil {
			return err
		}
		u.Details = wargamesMode
		break
	case CampaignGameMode: // Campaign
		var campaignMode GameModeCampaign
		err = json.Unmarshal(data, &campaignMode)
		if err != nil {
			return err
		}
		u.Details = campaignMode
		break
	case SpartanOpsGameMode: // Spartan Ops
		var spartanOpsMode GameModeSpartanOps
		err = json.Unmarshal(data, &spartanOpsMode)
		if err != nil {
			return err
		}
		u.Details = spartanOpsMode
		break
	}

	return nil
}

// MarshalJSON converts the custom data format back into the JSON format 343 uses >_>
func (u *GameModeBase) MarshalJSON() ([]byte, error) {
	data, err := json.Marshal(u.Details)
	if err != nil {
		return data, err
	}

	// Create custom JSON string with Game Mode ID and Name
	customJSONFormat := `"id":%d,"name":"%s",`
	customJSON := fmt.Sprintf(customJSONFormat, u.ID, u.Name)
	return helpers.ByteSliceInsert(data, []byte(customJSON), 1)
}
