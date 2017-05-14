package app

import (
	"github.com/branch-app/branch-mono-go/clients/auth"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
	"github.com/branch-app/branch-mono-go/services/halo4/services/waypoint"
)

// App handles business logic, does not involve HTTP
type App interface {
	GetServiceRecord(gamertag string) (*response.ServiceRecord, *log.E)
}

type app struct {
	authClient *auth.Client

	waypointService *waypoint.Client
}

// NewApp creates a new Xbox Live app.
func NewApp(authClient *auth.Client, waypointService *waypoint.Client) *app {
	return &app{
		authClient:      authClient,
		waypointService: waypointService,
	}
}
