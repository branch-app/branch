package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
)

func init() {
	routing.RegisterMethod("get_playercard", GetPlayerCard, "", []string{
		"2017-06-22",
	})
}

func GetPlayerCard(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	var identityLookup xboxlive.IdentityLookup
	if err := routing.ParseInput(c, &identityLookup, "get-playercard"); err != nil {
		return err
	}

	playerCard, err := app.GetPlayerCard(identityLookup)
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &playerCard)
}
