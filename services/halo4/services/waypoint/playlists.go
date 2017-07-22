package waypoint

import (
	"time"

	"github.com/branch-app/branch-mono-go/domain/branch"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

const (
	playlistsCollection = "playlists"
	playlistsURL        = "h4/playlists"
)

// GetPlaylists gets the Halo 4 playlist information.
func (client *Client) GetPlaylists() (*response.Playlists, *log.E) {
	jsonClient := client.presenceClient
	url, hash := client.constructURL(jsonClient, playlistsURL)
	var playlists *response.Playlists
	cached, err := client.Do(jsonClient, "GET", playlistsURL, playlistsCollection, nil, nil, &playlists)
	if err != nil {
		return nil, err
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		playlists.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, playlists, playlistsCollection)

	// Return identity
	return playlists, nil
}
