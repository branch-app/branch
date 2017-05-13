package contexts

import (
	"github.com/branch-app/service-halo4/clients"
	"github.com/branch-app/service-halo4/models"
	sharedClients "github.com/branch-app/shared-go/clients"
	sharedContexts "github.com/branch-app/shared-go/contexts"
)

// ServiceContext contains the service's context
type ServiceContext struct {
	sharedContexts.ServiceContext

	// HTTPClient is an http client to aid connecting to external APIs.
	HTTPClient *sharedClients.HTTPClient

	// ServiceClient is a client to aid connecting between services.
	ServiceClient *sharedClients.ServiceClient

	// ServiceClient is a client to aid connecting to xbox live.
	Halo4Client *clients.Halo4Client

	// Configuration holds the service's configuration information.
	Configuration *models.Configuration
}
