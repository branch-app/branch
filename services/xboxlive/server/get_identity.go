package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/xboxlive/app"
)

func init() {
	routing.RegisterMethod("get_identity", GetIdentity, "", []string{
		"2017-05-13",
	})
}

func GetIdentity(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	var identityLookup xboxlive.IdentityLookup
	if err := routing.ParseInput(c, &identityLookup, "get-identity"); err != nil {
		return err
	}

	identity, err := app.GetIdentity(identityLookup)
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &identity)
}
