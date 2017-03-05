package xboxlive

import (
	"github.com/branch-app/shared-go/models/branch"
	"github.com/maxwellhealth/bongo"
)

type ResponseCode int

const (
	ResponseOK              ResponseCode = 0
	ResponseUserDoesntExist ResponseCode = 28
)

type Response struct {
	bongo.DocumentBase `bson:",inline" json:"-"`
	branch.Response    `bson:"-"`

	Code             ResponseCode `bson:"code" json:"code"`
	Source           *string      `bson:"source" json:"source,omitempty"`
	Description      *string      `bson:"description" json:"description,omitempty"`
	TraceInformation *string      `bson:"tranceInformation" json:"tranceInformation,omitempty"`
}
