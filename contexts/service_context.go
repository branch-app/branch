package contexts

import (
	"github.com/TheTree/service-xboxlive/helpers"
	"github.com/TheTree/shared-go/contexts"
)

// ServiceContext contains the service's context
type ServiceContext struct {
	contexts.ServiceContext

	// XboxLiveStore is a store holding a key-value maps for Gamertag and XUIDs.
	XboxLiveStore *helpers.XboxLiveStore
}
