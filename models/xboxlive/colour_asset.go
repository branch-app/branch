package xboxlive

import (
	sharedClients "github.com/branch-app/shared-go/clients"
	"github.com/maxwellhealth/bongo"
	"gopkg.in/mgo.v2/bson"
)

type ColourAsset struct {
	Response `bson:",inline"`

	PrimaryColour   string `json:"primaryColor" bson:"primary_colour"`
	SecondaryColour string `json:"secondaryColor" bson:"secondary_colour"`
	TertiaryColour  string `json:"tertiaryColor" bson:"tertiary_colour"`
}

const colourAssetCollectionName = "colour_assets"

func (record *ColourAsset) Save(mongo *sharedClients.MongoDBClient) error {
	return mongo.Collection(colourAssetCollectionName).Save(record)
}

func ColourAssetFindOne(mongo *sharedClients.MongoDBClient, query bson.M) (*ColourAsset, error) {
	var colourAsset *ColourAsset
	err := mongo.Collection(colourAssetCollectionName).FindOne(query, &colourAsset)
	if err != nil {
		if _, ok := err.(*bongo.DocumentNotFoundError); ok {
			return nil, nil
		}

		return nil, err
	}

	return colourAsset, nil
}
