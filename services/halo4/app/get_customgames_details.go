package app

import (
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

func (a *app) GetCustomGamesDetails(identityLookup xboxlive.IdentityLookup) (*response.ModeDetailsWarGames, *log.E) {
	identity, err := a.xboxliveClient.GetIdentity(identityLookup)
	if err != nil {
		return nil, err
	}

	return a.waypointService.GetCustomGamesDetails(*identity)
}
