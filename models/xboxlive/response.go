package xboxlive

import (
	sharedModels "github.com/branch-app/shared-go/models"
)

type ResponseCode int

const (
	ResponseOK ResponseCode = 0
)

type Response struct {
	sharedModels.MongoAudit `bson:",inline" json:"-"`
	Code                    ResponseCode `bson:"code" json:"code"`
	Source                  string       `bson:"source" json:"source"`
	Description             string       `bson:"description" json:"description"`
	TraceInformation        string       `bson:"tranceInformation" json:"trance_information"`
}
