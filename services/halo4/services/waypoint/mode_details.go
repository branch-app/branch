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
	modeDetailsCollection     = "mode_details"
	campaignModeDetailsURL    = "players/%s/h4/servicerecord/campaign"
	spartanOpsModeDetailsURL  = "players/%s/h4/servicerecord/spartanops"
	warGamesModeDetailsURL    = "players/%s/h4/servicerecord/wargames"
	customGamesModeDetailsURL = "players/%s/h4/servicerecord/custom"
)

// GetCampaignDetails gets the campaign details of a player.
func (client *Client) GetCampaignDetails(identity xboxlive.Identity) (*response.ModeDetailsCampaign, *log.E) {
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(campaignModeDetailsURL, identity.Gamertag)
	url, hash := client.constructURL(jsonClient, endpoint)
	var campaignDetails *response.ModeDetailsCampaign
	cached, err := client.Do(jsonClient, "GET", endpoint, modeDetailsCollection, nil, nil, &campaignDetails)
	if err != nil {
		return nil, err
	}

	// Set cache data inside campaignDetails, if required
	if !cached {
		campaignDetails.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, campaignDetails, modeDetailsCollection)

	// Return campaign details
	return campaignDetails, nil
}

// GetCustomGamesDetails gets the campaign details of a player.
func (client *Client) GetCustomGamesDetails(identity xboxlive.Identity) (*response.ModeDetailsWarGames, *log.E) {
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(customGamesModeDetailsURL, identity.Gamertag)
	url, hash := client.constructURL(jsonClient, endpoint)
	var customGamesDetails *response.ModeDetailsWarGames
	cached, err := client.Do(jsonClient, "GET", endpoint, modeDetailsCollection, nil, nil, &customGamesDetails)
	if err != nil {
		return nil, err
	}

	// Set cache data inside customGamesDetails, if required
	if !cached {
		customGamesDetails.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, customGamesDetails, modeDetailsCollection)

	// Return custom games details
	return customGamesDetails, nil
}

// GetSpartanOpsDetails gets the campaign details of a player.
func (client *Client) GetSpartanOpsDetails(identity xboxlive.Identity) (*response.ModeDetailsSpartanOps, *log.E) {
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(spartanOpsModeDetailsURL, identity.Gamertag)
	url, hash := client.constructURL(jsonClient, endpoint)
	var spartanOpsDetails *response.ModeDetailsSpartanOps
	cached, err := client.Do(jsonClient, "GET", endpoint, modeDetailsCollection, nil, nil, &spartanOpsDetails)
	if err != nil {
		return nil, err
	}

	// Set cache data inside spartanOpsDetails, if required
	if !cached {
		spartanOpsDetails.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, spartanOpsDetails, modeDetailsCollection)

	// Return spartan ops details
	return spartanOpsDetails, nil
}

// GetWarGamesDetails gets the campaign details of a player.
func (client *Client) GetWarGamesDetails(identity xboxlive.Identity) (*response.ModeDetailsWarGames, *log.E) {
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(warGamesModeDetailsURL, identity.Gamertag)
	url, hash := client.constructURL(jsonClient, endpoint)
	var warGamesDetails *response.ModeDetailsWarGames
	cached, err := client.Do(jsonClient, "GET", endpoint, modeDetailsCollection, nil, nil, &warGamesDetails)
	if err != nil {
		return nil, err
	}

	// Set cache data inside warGamesDetails, if required
	if !cached {
		warGamesDetails.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, warGamesDetails, modeDetailsCollection)

	// Return war games details
	return warGamesDetails, nil
}
