package halo4

import (
	"time"

	sharedModels "github.com/branch-app/shared-go/models"
	mgo "gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

// Options defines the structure of the Service Record response.
type Options struct {
	Response `bson:",inline"`

	// Identifier is the identifier of the Halo Waypoint web app.
	Identifier string `json:"identifier" bson:"identifier"`

	// ServiceList is a list of service endpoints.
	ServiceList map[string]string `json:"serviceList" bson:"serviceList"`

	// Settings is a list of application dependant settings.
	Settings map[string]string `json:"settings" bson:"settings"`
}

const OptionsCollectionName = "options"

func (options *Options) Upsert(db *mgo.Database, url string, validFor time.Duration) error {
	now := time.Now().UTC()

	// Check if we need to set CreatedAt and ID
	if !options.BranchResponse.ID.Valid() {
		options.BranchResponse.ID = bson.NewObjectId()
		options.BranchResponse.CreatedAt = now
	}

	options.BranchResponse.UpdatedAt = now
	options.BranchResponse.CacheInformation = sharedModels.NewCacheInformation(url, now, validFor)
	_, err := db.C(OptionsCollectionName).UpsertId(options.BranchResponse.ID, options)
	return err
}

func OptionsFindOne(db *mgo.Database, id bson.ObjectId) *Options {
	var document *Options
	err := db.C(OptionsCollectionName).FindId(id).One(&document)
	switch {
	case err == nil:
		return document
	case err.Error() == "not found":
		return nil
	default:
		panic(err)
	}
}
