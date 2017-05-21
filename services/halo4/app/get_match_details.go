package app

import (
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/request"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

func (a *app) GetMatchDetails(request request.GetMatchDetails) (*response.Match, *log.E) {
	return a.waypointService.GetMatchDetails(request.MatchID)
}
