package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
	"github.com/branch-app/branch-mono-go/services/halo4/models/request"
)

func init() {
	routing.RegisterMethod("get_recent_matches", GetRecentMatches, "", []string{
		"2017-05-21",
	})
}

func GetRecentMatches(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	var request request.GetRecentMatches
	if err := routing.ParseInput(c, &request, "get-recent-matches"); err != nil {
		return err
	}

	serviceRecord, err := app.GetRecentMatches(request)
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &serviceRecord)
}
