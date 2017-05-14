package branch

import (
	"time"

	"gopkg.in/mgo.v2/bson"
)

// Response is the base structure every response should be inherited from.
type Response struct {
	CacheInformation *CacheInformation `json:"cacheInformation" bson:"cacheInformation"`

	ID        bson.ObjectId `json:"id" bson:"_id,omitempty"`
	CreatedAt time.Time     `json:"createdAt" bson:"createdAt"`
	UpdatedAt time.Time     `json:"updatedAt" bson:"updatedAt"`
}

// NewResponse creates a branch flavoured response and populates it with the relevant
// information.
func NewResponse(url string, cachedAt time.Time, validDuration time.Duration) Response {
	return Response{
		CacheInformation: NewCacheInformation(url, cachedAt, validDuration),
		ID:               bson.NewObjectId(),
		CreatedAt:        cachedAt,
		UpdatedAt:        cachedAt,
	}
}
