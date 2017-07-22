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
	matchesCollection       = "matches"
	recentMatchesCollection = "recent_matches"
	recentMatchesURL        = "players/%s/h4/matches?gamemodeid=%d&startat=%d&count=%d"
	matchesURL              = "h4/matches/%s"
)

// GetRecentMatches gets the recent matches of a player.
func (client *Client) GetRecentMatches(identity xboxlive.Identity, gameMode response.GameMode, startAt, count uint) (*response.RecentMatches, *log.E) {
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

// GetMatchDetails gets the match details by it's id.
func (client *Client) GetMatchDetails(matchID string) (*response.Match, *log.E) {
	// Get from xbox live
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(matchesURL, matchID)
	url, hash := client.constructURL(jsonClient, endpoint)
	var match *response.Match
	cached, err := client.Do(jsonClient, "GET", endpoint, matchesCollection, nil, nil, &match)
	if err != nil {
		return nil, err
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		match.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), (24*time.Hour)*7)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, match, recentMatchesCollection)

	// Return identity
	return match, nil
}
