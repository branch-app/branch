package app

import (
	"github.com/branch-app/branch-mono-go/clients/auth"
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	xblService "github.com/branch-app/branch-mono-go/services/xboxlive/services/xboxlive"
)

// App handles business logic, does not involve HTTP
type App interface {
	GetIdentity(identityLookup xboxlive.IdentityLookup) (*xboxlive.Identity, *log.E)
}

type app struct {
	authClient *auth.Client

	xblService *xblService.Client
}

// NewApp creates a new Xbox Live app.
func NewApp(authClient *auth.Client, xblService *xblService.Client) *app {
	return &app{
		authClient: authClient,
		xblService: xblService,
	}
}
