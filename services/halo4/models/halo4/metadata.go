package halo4

import (
	"time"

	sharedModels "github.com/branch-app/shared-go/models"
	mgo "gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

// Metadata defines the structure of the Metadata response.
type Metadata struct {
	Response `bson:",inline"`

	// DifficultiesMetadata contains the Difficulty Level metadata.
	DifficultiesMetadata DifficultiesMetadata `json:"difficultiesMetadata" bson:"difficultiesMetadata"`

	// SpartanOpsMetadata
	SpartanOpsMetadata SpartanOpsMetadata `json:"spartanOpsMetadata" bson:"spartanOpsMetadata"`
}

// DifficultiesMetadata defines the structure of the Difficulties metadata response.
type DifficultiesMetadata struct {
	// Difficulties is an array of all the difficulty levels.
	Difficulties []Difficulty `json:"difficulties" bson:"difficulties"`
}

// SpartanOpsMetadata defines the structure of the Spartan Ops metadata response.
type SpartanOpsMetadata struct {
	// SeasonsReleasedToDate holds the count of the currently released seasons.
	SeasonsReleasedToDate int `json:"seasonsReleasedToDate" bson:"seasonsReleasedToDate"`

	// ChaptersCurrentlyAvailable holds the count of the available chapters.
	ChaptersCurrentlyAvailable int `json:"chaptersCurrentlyAvailable" bson:"chaptersCurrentlyAvailable"`

	// BumperType unk
	BumperType int `json:"bumperType" bson:"bumperType"`

	// CurrentSeason holds the current active season.
	CurrentSeason int `json:"currentSeason" bson:"currentSeason"`

	// CurrentEpisode holds the current active episode.
	CurrentEpisode int `json:"currentEpisode" bson:"currentEpisode"`

	// Seasons holds a list of Spartan Ops seasons.
	Seasons []Season `json:"seasons" bson:"seasons"`
}

const MetadataCollectionName = "metadata"

func (metadata *Metadata) Upsert(db *mgo.Database, url string, validFor time.Duration) error {
	now := time.Now().UTC()

	// Check if we need to set CreatedAt and ID
	if !metadata.BranchResponse.ID.Valid() {
		metadata.BranchResponse.ID = bson.NewObjectId()
		metadata.BranchResponse.CreatedAt = now
	}

	metadata.BranchResponse.UpdatedAt = now
	metadata.BranchResponse.CacheInformation = sharedModels.NewCacheInformation(url, now, validFor)
	_, err := db.C(MetadataCollectionName).UpsertId(metadata.BranchResponse.ID, metadata)
	return err
}

func MetadataFindOne(db *mgo.Database, id bson.ObjectId) *Metadata {
	var document *Metadata
	err := db.C(MetadataCollectionName).FindId(id).One(&document)
	switch {
	case err == nil:
		return document
	case err.Error() == "not found":
		return nil
	default:
		panic(err)
	}
}
