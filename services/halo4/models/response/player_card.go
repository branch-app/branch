package response

import (
	"encoding/json"
	"fmt"

	"github.com/branch-app/shared-go/helpers"
)

// PlayerCard defines the structure of the Player Card action.
type PlayerCard struct {
	Response `bson:",inline"`

	// Gamertag is the Gamertag of the player.
	Gamertag string `json:"gamertag" bson:"gamertag"`

	// ServiceTag is the player's Service Tag.
	ServiceTag string `json:"serviceTag" bson:"serviceTag"`

	// EmblemImageURL is the asset of the players emblem.
	EmblemImageURL Asset `json:"emblemImageUrl" bson:"emblemImageUrl"`

	// BackgroundImageURL is the asset of the players ...
	BackgroundImageURL Asset `json:"backgroundImageUrl" bson:"backgroundImageUrl"`

	// FavoriteWeaponID is the ID of the players most used weapon.
	FavoriteWeaponID int `json:"favoriteWeaponId" bson:"favoriteWeaponId"`

	// FavoriteWeaponName is the name of the players most used weapon.
	FavoriteWeaponName string `json:"favoriteWeaponName" bson:"favoriteWeaponName"`

	// FavoriteWeaponDescription is the description of the players most used weapon.
	FavoriteWeaponDescription string `json:"favoriteWeaponDescription" bson:"favoriteWeaponDescription"`

	// FavoriteWeaponImageURL is the asset of the players most used weapon.
	FavoriteWeaponImageURL Asset `json:"favoriteWeaponImageUrl" bson:"favoriteWeaponImageUrl"`

	// FavoriteWeaponTotalKills is the number of kills the player has gotten with their
	// most used weapon.
	FavoriteWeaponTotalKills int `json:"favoriteWeaponTotalKills" bson:"favoriteWeaponTotalKills"`

	// RankID is the ID of the players rank.
	RankID int `json:"rankId" bson:"rankId"`

	// RankName is the name of the players rank.
	RankName string `json:"rankName" bson:"rankName"`

	// RankImageURL is the asset of the players rank.
	RankImageURL Asset `json:"rankImageUrl" bson:"rankImageUrl"`

	// RankStartXP is the amount of XP required to obtain this rank.
	RankStartXP uint64 `json:"rankStartXp" bson:"rankStartXp"`

	// NextRankStartXP is the amount of XP required to obtain the next rank.
	NextRankStartXP uint64 `json:"nextRankStartXp" bson:"nextRankStartXp"`

	// XP is th total amount of XP obtained by the player.
	XP uint64 `json:"xp" bson:"xp"`

	// NextRankID is the ID of the players next rank.
	NextRankID int `json:"nextRankId" bson:"nextRankId"`

	// NextRankName is the name of the players next rank.
	NextRankName string `json:"nextRankName" bson:"nextRankName"`

	// NextRankImageURL is the asset of the players next rank.
	NextRankImageURL *Asset `json:"nextRankImageUrl" bson:"nextRankImageUrl"`

	// TopMedals are the top medals earned by the player.
	TopMedals []TopMedal `json:"topMedals" bson:"topMedals"`

	// Specializations contain the specializations progression of the player.
	Specializations []Specialization `json:"specializations" bson:"specializations"`

	// GameModes contains information about the player in each of the games modes.
	GameModes []GameModeBase `json:"gameModes" bson:"gameModes"`

	// TopSkillRank is the players highest earned Skill Rank.
	TopSkillRank *SkillRank `json:"topSkillRank" bson:"topSkillRank"`
}

// PlayerCardGameModeBase defines the structure of the game mode in a player card.
type PlayerCardGameModeBase struct {
	ID      GameMode    `json:"id" bson:"id"`
	Name    string      `json:"name" bson:"name"`
	Details interface{} `json:"details" bson:"details"`
}

// PlayerCardGameModeWarGames defines the structure of the war games mode in a player card.
type PlayerCardGameModeWarGames struct {
	AveragePersonalScore       int     `json:"averagePersonalScore" bson:"averagePersonalScore"`
	KDRatio                    float64 `json:"kdRatio" bson:"kdRatio"`
	TotalGameBaseVariantMedals int     `json:"totalGameBaseVariantMedals" bson:"totalGameBaseVariantMedals"`
	FavoriteVariant            Asset   `json:"favoriteVariant" bson:"favoriteVariant"`
	PresentationID             int     `json:"presentationId" bson:"presentationId"`
}

// PlayerCardGameModeGeneric defines the structure of the a generic game mode in a player card.
type PlayerCardGameModeGeneric struct {
	TotalGamesStarted int `json:"totalGamesStarted" bson:"totalGamesStarted"`
}

// UnmarshalJSON unmarshals the generic Game Modes struct into a mode-specific struct
func (u *PlayerCardGameModeBase) UnmarshalJSON(data []byte) error {
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

	// Set alias data to PlayerCardGameModeBase
	u.ID = alias.ID
	u.Name = alias.Name

	switch u.ID {
	case WarGamesGameMode: // War Games
		var wargamesMode PlayerCardGameModeWarGames
		err = json.Unmarshal(data, &wargamesMode)
		if err != nil {
			return err
		}
		u.Details = wargamesMode
		break
	default: // Campaign, Spartan Ops, and Customs
		var genericMode PlayerCardGameModeGeneric
		err = json.Unmarshal(data, &genericMode)
		if err != nil {
			return err
		}
		u.Details = genericMode
		break
	}

	return nil
}

// MarshalJSON converts the custom data format back into the JSON format 343 uses >_>
func (u *PlayerCardGameModeBase) MarshalJSON() ([]byte, error) {
	data, err := json.Marshal(u.Details)
	if err != nil {
		return data, err
	}

	// Create custom JSON string with Game Mode ID and Name
	customJSONFormat := `"id":%d,"name":"%s",`
	customJSON := fmt.Sprintf(customJSONFormat, u.ID, u.Name)
	return helpers.ByteSliceInsert(data, []byte(customJSON), 1)
}
