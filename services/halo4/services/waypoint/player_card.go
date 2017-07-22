package waypoint

import (
	"time"

	"fmt"

	"github.com/branch-app/branch-mono-go/domain/branch"
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
)

const (
	playerCardsCollection = "player_cards"
	playerCardsURL        = "players/%s/h4/playercard"
)

// GetPlayerCard gets a player card for the identity specified.
func (client *Client) GetPlayerCard(identity xboxlive.Identity) (*response.PlayerCard, *log.E) {
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(playerCardsURL, identity.Gamertag)
	url, hash := client.constructURL(jsonClient, endpoint)
	var playerCard *response.PlayerCard
	cached, err := client.Do(jsonClient, "GET", endpoint, playerCardsCollection, nil, nil, &playerCard)
	if err != nil {
		return nil, err
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		playerCard.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, playerCard, playerCardsCollection)

	// Return identity
	return playerCard, nil
}
