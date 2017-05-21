package waypoint

import (
	"fmt"

	"time"

	"github.com/branch-app/branch-mono-go/domain/branch"
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

const (
	profileCollection = "profile_users"
	serviceRecordURL  = "players/%s/h4/servicerecord"
)

// GetServiceRecord gets the service record of a specified player.
func (client *Client) GetServiceRecord(identity xboxlive.Identity) (*response.ServiceRecord, *log.E) {
	// Get from xbox live
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(serviceRecordURL, identity.Gamertag)
	url, hash := client.constructURL(jsonClient, endpoint)
	var serviceRecord *response.ServiceRecord
	cached, err := client.Do(jsonClient, "GET", endpoint, profileCollection, nil, nil, &serviceRecord)
	if err != nil {
		return nil, err
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		serviceRecord.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, serviceRecord, profileCollection)

	// Return identity
	return serviceRecord, nil
}
