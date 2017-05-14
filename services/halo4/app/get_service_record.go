package app

import (
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

func (a *app) GetServiceRecord(identityLookup xboxlive.IdentityLookup) (*response.ServiceRecord, *log.E) {
	identity, err := a.xboxliveClient.GetIdentity(identityLookup)
	if err != nil {
		return nil, err
	}

	return a.waypointService.GetServiceRecord(*identity)
}
