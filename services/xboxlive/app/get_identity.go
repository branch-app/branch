package app

import (
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
)

func (a *app) GetIdentity(identityLookup xboxlive.IdentityLookup) (*xboxlive.Identity, *log.E) {
	identity, err := a.xblService.GetIdentity(identityLookup.Value, identityLookup.Type)

	return identity, err
}
