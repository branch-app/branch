package app

import (
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

func (a *app) GetServiceRecord(gamertag string) (*response.ServiceRecord, *log.E) {
	x := xboxlive.Identity{Gamertag: gamertag}
	return a.waypointService.GetServiceRecord(x)
}
