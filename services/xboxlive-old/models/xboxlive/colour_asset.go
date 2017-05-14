package xboxlive

import (
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"

	"time"

	sharedModels "github.com/branch-app/shared-go/models"
)

type ColourAsset struct {
	Response `bson:",inline"`

	PrimaryColour   string `json:"primaryColor" bson:"primaryColour"`
	SecondaryColour string `json:"secondaryColor" bson:"secondaryColour"`
	TertiaryColour  string `json:"tertiaryColor" bson:"tertiaryColour"`
}

const ColourAssetsCollectionName = "colour_assets"

func (colourAsset *ColourAsset) Upsert(db *mgo.Database, url string, validFor time.Duration) error {
	now := time.Now().UTC()

	// Check if we need to set CreatedAt and ID
	if !colourAsset.BranchResponse.ID.Valid() {
		colourAsset.BranchResponse.ID = bson.NewObjectId()
		colourAsset.BranchResponse.CreatedAt = now
	}

	colourAsset.BranchResponse.UpdatedAt = now
	colourAsset.BranchResponse.CacheInformation = sharedModels.NewCacheInformation(url, now, validFor)
	_, err := db.C(ColourAssetsCollectionName).UpsertId(colourAsset.BranchResponse.ID, colourAsset)
	return err
}

func ColourAssetFindOne(db *mgo.Database, id bson.ObjectId) *ColourAsset {
	var document *ColourAsset
	err := db.C(ColourAssetsCollectionName).FindId(id).One(&document)
	switch {
	case err == nil:
		return document
	case err.Error() == "not found":
		return nil
	default:
		panic(err)
	}
}
