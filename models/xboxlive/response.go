package xboxlive

import "github.com/maxwellhealth/bongo"

type ResponseCode int

const (
	ResponseOK              ResponseCode = 0
	ResponseUserDoesntExist ResponseCode = 28
)

type Response struct {
	bongo.DocumentBase `bson:",inline" json:"-"`
	Code               ResponseCode `bson:"code" json:"code"`
	Source             string       `bson:"source" json:"source"`
	Description        string       `bson:"description" json:"description"`
	TraceInformation   *string      `bson:"tranceInformation" json:"trance_information"`
}
