package halo4

import (
	sharedModels "github.com/branch-app/shared-go/models"
	mgo "gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

type Response struct {
	sharedModels.BranchResponse `bson:",inline"`

	// StatusCode defines the status of the response.
	StatusCode StatusCode `json:"statusCode" bson:"statusCode"`

	// StatusReason is the reason the response failed if the StatusCode is not 1.
	StatusReason string `json:"statusReason" bson:"statusReason"`

	// DateFidelity defines the levels of fidelity of FirstPlayed and LastPlayed.
	DateFidelity Fidelity `json:"dateFidelity" bson:"dateFidelity"`
}

func GetCacheInfo(db *mgo.Database, collection, urlHash string) *sharedModels.BranchResponse {
	var document Response
	err := db.C(collection).Find(bson.M{"cacheInformation.docUrlHash": urlHash}).Select(bson.M{"cacheInformation": 1}).One(&document)
	switch {
	case err == nil:
		return &document.BranchResponse
	case err.Error() == "not found":
		return nil
	default:
		panic(err)
	}
}
