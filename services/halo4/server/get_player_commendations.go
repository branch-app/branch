package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
)

func init() {
	routing.RegisterMethod("get_player_commendations", GetPlayerCommendations, "", []string{
		"2017-07-16",
	})
}

func GetPlayerCommendations(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	var identityLookup xboxlive.IdentityLookup
	if err := routing.ParseInput(c, &identityLookup, "get-player-commendations"); err != nil {
		return err
	}

	commendations, err := app.GetPlayerCommendations(identityLookup)
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &commendations)
}
