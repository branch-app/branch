package app

import (
	"github.com/branch-app/branch-mono-go/clients/auth"
	"github.com/branch-app/branch-mono-go/clients/xboxlive"
	xblDomain "github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/request"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
	"github.com/branch-app/branch-mono-go/services/halo4/services/waypoint"
)

// App handles business logic, does not involve HTTP
type App interface {
	GetServiceRecord(request xblDomain.IdentityLookup) (*response.ServiceRecord, *log.E)
	GetRecentMatches(request request.RecentMatches) (*response.RecentMatches, *log.E)
}

type app struct {
	authClient     *auth.Client
	xboxliveClient *xboxlive.Client

	waypointService *waypoint.Client
}

// NewApp creates a new Xbox Live app.
func NewApp(authClient *auth.Client, xboxliveClient *xboxlive.Client, waypointService *waypoint.Client) *app {
	return &app{
		authClient:     authClient,
		xboxliveClient: xboxliveClient,

		waypointService: waypointService,
	}
}
