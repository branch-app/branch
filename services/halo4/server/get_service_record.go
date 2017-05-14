package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
	"github.com/branch-app/branch-mono-go/services/halo4/models/request"
)

func init() {
	routing.RegisterMethod("get_service_record", GetServiceRecord, "", []string{
		"2017-05-13",
	})
}

func GetServiceRecord(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	var identityLookup request.Identity
	if err := routing.ParseInput(c, &identityLookup, "get-service-record"); err != nil {
		return err
	}

	identity, err := app.GetServiceRecord(identityLookup.Value)
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &identity)
}
