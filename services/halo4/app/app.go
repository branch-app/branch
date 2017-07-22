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
	GetCampaignDetails(request xblDomain.IdentityLookup) (*response.ModeDetailsCampaign, *log.E)
	GetCustomGamesDetails(request xblDomain.IdentityLookup) (*response.ModeDetailsWarGames, *log.E)
	GetGlobalChallenges() (*response.Challenges, *log.E)
	GetMatchDetails(request request.GetMatchDetails) (*response.Match, *log.E)
	GetMetadata(request request.GetMetadata) (*response.Metadata, *log.E)
	GetOptions() (*response.Options, *log.E)
	GetPlayerCard(request xblDomain.IdentityLookup) (*response.PlayerCard, *log.E)
	GetPlayerCommendations(request xblDomain.IdentityLookup) (*response.Commendations, *log.E)
	GetPlaylists() (*response.Playlists, *log.E)
	GetRecentMatches(request request.GetRecentMatches) (*response.RecentMatches, *log.E)
	GetServiceRecord(request xblDomain.IdentityLookup) (*response.ServiceRecord, *log.E)
	GetSpartanOpsDetails(request xblDomain.IdentityLookup) (*response.ModeDetailsSpartanOps, *log.E)
	GetWarGamesDetails(request xblDomain.IdentityLookup) (*response.ModeDetailsWarGames, *log.E)
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
