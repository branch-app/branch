package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
)

func init() {
	routing.RegisterMethod("get_service_record", GetServiceRecord, "", []string{
		"2017-05-13",
	})
}

func GetServiceRecord(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	var identityLookup xboxlive.IdentityLookup
	if err := routing.ParseInput(c, &identityLookup, "get-service-record"); err != nil {
		return err
	}

	serviceRecord, err := app.GetServiceRecord(identityLookup)
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &serviceRecord)
}
