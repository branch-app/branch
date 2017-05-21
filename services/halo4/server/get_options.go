package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
)

func init() {
	routing.RegisterMethod("get_options", GetOptions, "", []string{
		"2017-05-21",
	})
}

func GetOptions(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	options, err := app.GetOptions()
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &options)
}
