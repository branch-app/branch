package contexts

import "github.com/TheTree/service-xboxlive/helpers"

// ServiceContext contains the service's context
type ServiceContext struct {
	// ServiceID is the ID of the service that owns the context.
	ServiceID string

	// XboxLiveStore is a store holding a key-value maps for Gamertag and XUIDs.
	XboxLiveStore *helpers.XboxLiveStore
}
