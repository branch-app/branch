package xboxlive

import (
	sharedModels "github.com/branch-app/shared-go/models"
	mgo "gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

type ResponseCode int

const (
	ResponseOK              ResponseCode = 0
	ResponseUserDoesntExist ResponseCode = 28
)

type Response struct {
	sharedModels.BranchResponse `bson:",inline"`

	Code             ResponseCode `bson:"code" json:"code"`
	Source           *string      `bson:"source" json:"source"`
	Description      *string      `bson:"description" json:"description"`
	TraceInformation *string      `bson:"tranceInformation" json:"tranceInformation"`
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
