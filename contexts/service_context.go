package contexts

import (
	"github.com/TheTree/helpers-go/contexts"
	"github.com/TheTree/service-xboxlive/helpers"
)

// ServiceContext contains the service's context
type ServiceContext struct {
	contexts.ServiceContext

	// XboxLiveStore is a store holding a key-value maps for Gamertag and XUIDs.
	XboxLiveStore *helpers.XboxLiveStore
}
