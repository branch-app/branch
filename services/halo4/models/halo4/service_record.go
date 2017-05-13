package halo4

import (
	"time"

	sharedModels "github.com/branch-app/shared-go/models"
	mgo "gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

// ServiceRecord defines the structure of the Service Record response.
type ServiceRecord struct {
	Response `bson:",inline"`

	// LastPlayedUTC is when the player was last active.
	LastPlayedUTC time.Time `json:"lastPlayedUtc" bson:"lastPlayedUtc"`

	// FirstPlayedUTC is when the player first played.
	FirstPlayedUTC time.Time `json:"firstPlayedUtc" bson:"firstPlayedUtc"`

	// SpartanPoints is the total number of Spartan Points owned by the player.
	SpartanPoints uint `json:"spartanPoints" bson:"spartanPoints"`

	// TotalGamesStarted is the total number of games started by the player.
	TotalGamesStarted uint `json:"totalGamesStarted" bson:"totalGamesStarted"`

	// TotalMedalsEarned is the total number of medals earned by the player.
	TotalMedalsEarned uint `json:"totalMedalsEarned" bson:"totalMedalsEarned"`

	// TotalGameplay is the total length of time the player has played for.
	TotalGameplay string `json:"totalGameplay" bson:"totalGameplay"`

	// TotalChallengesCompleted is the total number of challenges completed by the player.
	TotalChallengesCompleted uint `json:"totalChallengesCompleted" bson:"totalChallengesCompleted"`

	// TotalLoadoutItemsPurchased is the number of loadout items purchased by the player.
	TotalLoadoutItemsPurchased uint `json:"totalLoadoutItemsPurchased" bson:"totalLoadoutItemsPurchased"`

	// TotalCommendationProgress is the total commendation progress made by the player.
	TotalCommendationProgress float64 `json:"totalCommendationProgress" bson:"totalCommendationProgress"`

	// SkillRanks is a list of the skill ranks of the player in each playlist.
	SkillRanks *[]SkillRank `json:"skillRanks" bson:"skillRanks"`

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

const ServiceRecordCollectionName = "service_records"

func (serviceRecord *ServiceRecord) Upsert(db *mgo.Database, url string, validFor time.Duration) error {
	now := time.Now().UTC()

	// Check if we need to set CreatedAt and ID
	if !serviceRecord.BranchResponse.ID.Valid() {
		serviceRecord.BranchResponse.ID = bson.NewObjectId()
		serviceRecord.BranchResponse.CreatedAt = now
	}

	serviceRecord.BranchResponse.UpdatedAt = now
	serviceRecord.BranchResponse.CacheInformation = sharedModels.NewCacheInformation(url, now, validFor)
	_, err := db.C(ServiceRecordCollectionName).UpsertId(serviceRecord.BranchResponse.ID, serviceRecord)
	return err
}

func ServiceRecordFindOne(db *mgo.Database, id bson.ObjectId) *ServiceRecord {
	var document *ServiceRecord
	err := db.C(ServiceRecordCollectionName).FindId(id).One(&document)
	switch {
	case err == nil:
		return document
	case err.Error() == "not found":
		return nil
	default:
		panic(err)
	}
}
