package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
	"github.com/branch-app/branch-mono-go/services/halo4/models/request"
)

func init() {
	routing.RegisterMethod("get_match_details", GetMatchDetails, "", []string{
		"2017-05-21",
	})
}

func GetMatchDetails(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	var matchID request.GetMatchDetails
	if err := routing.ParseInput(c, &matchID, "get-match-details"); err != nil {
		return err
	}

	matchDetails, err := app.GetMatchDetails(matchID)
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &matchDetails)
}
