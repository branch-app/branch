package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
)

func init() {
	routing.RegisterMethod("get_playlists", GetPlaylists, "", []string{
		"2017-05-21",
	})
}

func GetPlaylists(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	playlists, err := app.GetPlaylists()
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &playlists)
}
