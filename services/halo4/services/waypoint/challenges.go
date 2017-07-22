package waypoint

import (
	"time"

	"github.com/branch-app/branch-mono-go/domain/branch"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

const (
	challengesCollection = "challenges"
	globalChallengesURL  = "h4/challenges"
)

// GetGlobalChallenges gets the global challenge information.
func (client *Client) GetGlobalChallenges() (*response.Challenges, *log.E) {
	jsonClient := client.statsClient
	url, hash := client.constructURL(jsonClient, globalChallengesURL)
	var challenges *response.Challenges
	cached, err := client.Do(jsonClient, "GET", globalChallengesURL, challengesCollection, nil, nil, &challenges)
	if err != nil {
		return nil, err
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		challenges.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, challenges, challengesCollection)

	// Return identity
	return challenges, nil
}
