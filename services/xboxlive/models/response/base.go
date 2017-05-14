package response

import (
	"github.com/branch-app/branch-mono-go/domain/branch"
)

type ResponseCode int

const (
	ResponseOK              ResponseCode = 0
	ResponseUserDoesntExist ResponseCode = 28
)

type Base struct {
	branch.Response `bson:",inline"`

	Code             ResponseCode `bson:"code" json:"code"`
	Source           *string      `bson:"source" json:"source"`
	Description      *string      `bson:"description" json:"description"`
	TraceInformation *string      `bson:"tranceInformation" json:"tranceInformation"`
}
