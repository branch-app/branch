package server

import (
	"fmt"
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
	"github.com/branch-app/branch-mono-go/services/halo4/models/request"
)

func init() {
	routing.RegisterMethod("get_metadata", GetMetadata, "", []string{
		"2017-05-21",
	})
}

func GetMetadata(c *routing.Context, v int64) *log.E {
	app := c.App.(app.App)
	var request request.GetMetadata
	if err := routing.ParseInput(c, &request, "get-metadata"); err != nil {
		return err
	}

	fmt.Println(request)
	metadata, err := app.GetMetadata(request)
	if err != nil {
		return err
	}

	return routing.ParseOutput(c, http.StatusOK, &metadata)
}
