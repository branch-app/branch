package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/xboxlive/app"
	"github.com/branch-app/branch-mono-go/services/xboxlive/models/request"
)

func init() {
	routing.RegisterMethod("get_identity", GetIdentity, "", []string{
		"2017-05-13",
	})
}

func GetIdentity(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	var identityLookup request.Identity
	if err := routing.ParseInput(c, &identityLookup, "get-identity"); err != nil {
		return err
	}

	identity, err := app.GetIdentity(identityLookup)
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &identity)
}
