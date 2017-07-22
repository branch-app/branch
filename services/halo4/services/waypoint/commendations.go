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
	commendationsCollection = "commendations"
	playerCommendationsURL  = "players/%s/h4/commendations"
)

// GetPlayerCommendations gets the commendation information of a player.
func (client *Client) GetPlayerCommendations(identity xboxlive.Identity) (*response.Commendations, *log.E) {
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(playerCommendationsURL, identity.Gamertag)
	url, hash := client.constructURL(jsonClient, endpoint)
	var commendations *response.Commendations
	cached, err := client.Do(jsonClient, "GET", endpoint, commendationsCollection, nil, nil, &commendations)
	if err != nil {
		return nil, err
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		commendations.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, commendations, commendationsCollection)

	// Return identity
	return commendations, nil
}
