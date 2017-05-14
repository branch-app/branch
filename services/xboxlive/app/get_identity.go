package app

import (
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/xboxlive/models/request"
)

func (a *app) GetIdentity(identityLookup request.Identity) (*xboxlive.Identity, *log.E) {
	identity, err := a.xblService.GetIdentity(identityLookup.Value, identityLookup.Type)

	return identity, err
}
