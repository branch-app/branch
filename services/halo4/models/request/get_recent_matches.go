package request

import (
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

type RecentMatches struct {
	Identity xboxlive.IdentityLookup `json:"identity" bson:"identity"`

	GameModeID response.GameMode `json:"gameModeId" bson:"gameModeId"`
	StartAt    uint              `json:"startAt" bson:"startAt"`
	Count      uint              `json:"count" bson:"count"`
}
