package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
)

func init() {
	routing.RegisterMethod("get_global_challenges", GetGlobalChallenges, "", []string{
		"2017-05-21",
	})
}

func GetGlobalChallenges(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	challenges, err := app.GetGlobalChallenges()
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &challenges)
}
