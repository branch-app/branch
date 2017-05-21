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
	recentMatchesCollection = "recent_matches"
	recentMatchesURL        = "players/%s/h4/matches?gamemodeid=%d&startat=%d&count=%d"
)

// GetRecentMatches gets the recent matches
func (client *Client) GetRecentMatches(identity *xboxlive.Identity, gameMode response.GameMode, startAt, count uint) (*response.RecentMatches, *log.E) {
	// Get from xbox live
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(recentMatchesURL, identity.Gamertag, gameMode, startAt, count)
	url, hash := client.constructURL(jsonClient, endpoint)
	var recentMatches *response.RecentMatches
	cached, err := client.Do(jsonClient, "GET", endpoint, recentMatchesCollection, nil, nil, &recentMatches)
	if err != nil {
		return nil, err
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		recentMatches.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, recentMatches, recentMatchesCollection)

	// Return identity
	return recentMatches, nil
}
