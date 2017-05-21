package app

import (
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/request"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

func (a *app) GetRecentMatches(request request.GetRecentMatches) (*response.RecentMatches, *log.E) {
	identity, err := a.xboxliveClient.GetIdentity(request.Identity)
	if err != nil {
		return nil, err
	}

	return a.waypointService.GetRecentMatches(identity, request.GameModeID, request.StartAt, request.Count)
}
