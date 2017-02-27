package contexts

import (
	"github.com/branch-app/service-xboxlive/clients"
	"github.com/branch-app/service-xboxlive/models"
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
	XboxLiveClient *clients.XboxLiveClient

	// Configuration holds the service's configuration information.
	Configuration *models.Configuration
}
