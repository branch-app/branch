package response

import (
	"encoding/json"
	"fmt"
	"time"

	"github.com/branch-app/branch-mono-go/helpers"
)

// RecentMatches defines the structure of the Recent Matches response.
type RecentMatches struct {
	Response `bson:",inline"`

	// Games is a list of recent games the player has played.
	Games []RecentMatchBase `json:"games" bson:"games"`
}

// RecentMatchBase defines the custom base structure of recent matches.
type RecentMatchBase struct {
	ModeID  GameMode    `json:"modeId" bson:"modeId"`
	Details interface{} `json:"details" bson:"details"`
}

// RecentMatchShared defines the common variables that are used in every RecentMatchX.
type RecentMatchShared struct {
	// TopMedalIDs is a list of the top medal ids earned by the player.
	TopMedalIDs []uint `json:"topMedalIds" bson:"topMedalIds"`

	// PersonalScore is the players personal score in the match. Always 0 in campaign.
	PersonalScore int `json:"personalScore" bson:"personalScore"`

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
}

// RecentMatchWarGames defines the structure of recent War Games matches.
type RecentMatchWarGames struct {
	RecentMatchShared `bson:",inline"`

	// Standing is the standing of the player in the match.
	Standing int `json:"standing" bson:"standing"`

	// BaseVariantID is the id of the base game variant.
	BaseVariantID int `json:"baseVariantId" bson:"baseVariantId"`

	// BaseVariantImageURL is the asset of the base game variant.
	BaseVariantImageURL Asset `json:"baseVariantImageUrl" bson:"baseVariantImageUrl"`

	// VariantName is the name of the game variant.
	VariantName string `json:"variantName" bson:"variantName"`

	// FeaturedStatName is the name of the featured stat of the game.
	FeaturedStatName string `json:"featuredStatName" bson:"featuredStatName"`

	// FeaturedStatValue is the value of the featured stat of the game.
	FeaturedStatValue int `json:"featuredStatValue" bson:"featuredStatValue"`

	// TotalMedals is the total number if medals the player earned in the game.
	TotalMedals uint `json:"totalMedals" bson:"totalMedals"`

	// MapID is the identifier of the map played on.
	MapID int `json:"mapId" bson:"mapId"`

	// MapImageURL is the asset of the map played on.
	MapImageURL Asset `json:"mapImageUrl" bson:"mapImageUrl"`

	// MapVariantName is the name of the variant of the map played on.
	MapVariantName string `json:"mapVariantName" bson:"mapVariantName"`

	// PlaylistID is the identifier of the playlist the game was played in.
	PlaylistID int `json:"playlistId" bson:"playlistId"`

	// PlaylistName is the name of the playlist the game was played in.
	PlaylistName string `json:"playlistName" bson:"playlistName"`
}

// RecentMatchCampaign defines the structure of recent Campaign matches.
type RecentMatchCampaign struct {
	RecentMatchShared `bson:",inline"`

	// MapID is the identifier of the map played on.
	MapID int `json:"mapId" bson:"mapId"`

	// MapImageURL is the asset of the map played on.
	MapImageURL Asset `json:"mapImageUrl" bson:"mapImageUrl"`

	// DifficultyImageURL is the asset of the map played on.
	DifficultyImageURL Asset `json:"difficultyImageUrl" bson:"difficultyImageUrl"`

	// Difficulty is the difficulty the match was played on.
	Difficulty DifficultyLevel `json:"difficulty" bson:"difficulty"`

	// SkullIDs is a list of skulls that were turned on for the match.
	SkullIDs []uint `json:"skullIds" bson:"skullIds"`

	// Duration is the length of time the match was played for.
	Duration string `json:"duration" bson:"duration"`

	// SinglePlayer shows if the match was played alone or in coop.
	SinglePlayer bool `json:"singlePlayer" bson:"singlePlayer"`

	// Mission is the index of the mission (1 indexed)
	Mission uint `json:"mission" bson:"mission"`

	// MapName is the name of the map the match was played on.
	MapName string `json:"mapName" bson:"mapName"`
}

// RecentMatchSpartanOps defines the structure of recent Spartan Ops matches.
type RecentMatchSpartanOps struct {
	RecentMatchShared `bson:",inline"`

	// EpisodeName is the name of the Spartan Ops Episode.
	EpisodeName string `json:"episodeName" bson:"episodeName"`

	// ChapterName is the name of the Spartan Ops Chapter.
	ChapterName string `json:"chapterName" bson:"chapterName"`

	// Difficulty is the difficulty the match was played on.
	Difficulty DifficultyLevel `json:"difficulty" bson:"difficulty"`

	// DifficultyImageURL is the asset of the map played on.
	DifficultyImageURL Asset `json:"difficultyImageUrl" bson:"difficultyImageUrl"`

	// SinglePlayer shows if the match was played alone or in coop.
	SinglePlayer bool `json:"singlePlayer" bson:"singlePlayer"`

	// Duration is the length of time the match was played for.
	Duration string `json:"duration" bson:"duration"`

	// SeasonID is the identifier of the Spartan Ops Season.
	SeasonID uint `json:"seasonId" bson:"seasonId"`

	// EpisodeID is the identifier of the Spartan Ops Episode.
	EpisodeID uint `json:"episodeId" bson:"episodeId"`

	// ChapterID is the identifier of the Spartan Ops Chapter.
	ChapterID uint `json:"chapterId" bson:"chapterId"`

	// ChapterNumber is the number of the Spartan Ops Season.
	ChapterNumber uint `json:"chapterNumber" bson:"chapterNumber"`
}

// UnmarshalJSON unmarshals bullshit dynamic Recent Match JSON.
func (u *RecentMatchBase) UnmarshalJSON(data []byte) error {
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
		var wargamesMatch RecentMatchWarGames
		if err = json.Unmarshal(data, &wargamesMatch); err != nil {
			return err
		}
		u.Details = wargamesMatch
		break
	case CampaignGameMode: // Campaign
		var campaignMatch RecentMatchCampaign
		if err = json.Unmarshal(data, &campaignMatch); err != nil {
			return err
		}
		u.Details = campaignMatch
		break
	case SpartanOpsGameMode: // Spartan Ops
		var spartanOpsMatch RecentMatchSpartanOps
		if err = json.Unmarshal(data, &spartanOpsMatch); err != nil {
			return err
		}
		u.Details = spartanOpsMatch
		break
	}

	return nil
}

// MarshalJSON converts the custom data format back into the JSON format 343 uses >_>
func (u *RecentMatchBase) MarshalJSON() ([]byte, error) {
	data, err := json.Marshal(u.Details)
	if err != nil {
		return data, err
	}

	// Create custom JSON string with Game Mode ID and Name
	customJSONFormat := `"modeId":%d,`
	customJSON := fmt.Sprintf(customJSONFormat, u.ModeID)
	return helpers.ByteSliceInsert(data, []byte(customJSON), 1)
}
