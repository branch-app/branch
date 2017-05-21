package app

import (
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

func (a *app) GetOptions() (*response.Options, *log.E) {
	return a.waypointService.GetOptions()
}
