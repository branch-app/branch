package clients

import (
	"errors"
	"fmt"
	"time"

	"github.com/branch-app/shared-go/crypto"
	sharedModels "github.com/branch-app/shared-go/models"

	"github.com/branch-app/service-halo4/models/halo4"
)

const (
	recentMatchesURL = "https://stats.svc.halowaypoint.com/en-US/players/%s/h4/matches?gamemodeid=%d&startat=%d&count=%d"
	matchURL         = "https://stats.svc.halowaypoint.com/en-US/h4/matches/%s"
)

func (client *Halo4Client) GetRecentMatches(identity *sharedModels.XboxLiveIdentity, modeId, startAt, count int) (*halo4.RecentMatches, error) {
	// Validate ModeId
	if modeId < 3 || modeId > 6 {
		return nil, errors.New("mode_id_out_of_range")
	}

	// Validate Start At
	if startAt < 0 {
		return nil, errors.New("start_at_out_of_range")
	}

	// Validate Count
	if count <= 0 || count > 25 {
		return nil, errors.New("count_out_of_range")
	}

	url := fmt.Sprintf(recentMatchesURL, identity.Gamertag, modeId, startAt, count)
	urlHash := crypto.CreateSHA512Hash(url)
	auth, authErr := client.GetAuthentication()
	cacheInfo := halo4.GetCacheInfo(client.mongoClient.DB(), halo4.RecentMatchesCollectionName, urlHash)
	if cacheInfo != nil && (authErr != nil || cacheInfo.CacheInformation.IsValid()) {
		return halo4.RecentMatchesFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
	}
	if authErr != nil {
		return nil, client.handleError(nil, authErr)
	}

	// Retrieve data from Xbox Live
	var recentMatches *halo4.RecentMatches
	_, err := client.ExecuteRequest("GET", url, auth, nil, &recentMatches)
	if err != nil {
		if cacheInfo != nil {
			return halo4.RecentMatchesFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
		}
		return nil, client.handleError(&recentMatches.Response, err)
	}

	// Preserve ID and CreatedAt if we only updating
	if cacheInfo != nil {
		recentMatches.BranchResponse.ID = cacheInfo.ID
		recentMatches.BranchResponse.CreatedAt = cacheInfo.CreatedAt
	}

	if err := recentMatches.Upsert(client.mongoClient.DB(), url, 720*time.Hour); err != nil {
		panic(err)
	}
	return recentMatches, nil
}

func (client *Halo4Client) GetMatch(matchId string) (*halo4.Match, error) {
	url := fmt.Sprintf(matchURL, matchId)
	urlHash := crypto.CreateSHA512Hash(url)
	auth, authErr := client.GetAuthentication()
	cacheInfo := halo4.GetCacheInfo(client.mongoClient.DB(), halo4.MatchesCollectionName, urlHash)
	if cacheInfo != nil && (authErr != nil || cacheInfo.CacheInformation.IsValid()) {
		return halo4.MatchFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
	}
	if authErr != nil {
		return nil, client.handleError(nil, authErr)
	}

	// Retrieve data from Xbox Live
	var match *halo4.Match
	_, err := client.ExecuteRequest("GET", url, auth, nil, &match)
	if err != nil {
		if cacheInfo != nil {
			return halo4.MatchFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
		}
		return nil, client.handleError(&match.Response, err)
	}

	// Preserve ID and CreatedAt if we only updating
	if cacheInfo != nil {
		match.BranchResponse.ID = cacheInfo.ID
		match.BranchResponse.CreatedAt = cacheInfo.CreatedAt
	}

	if err := match.Upsert(client.mongoClient.DB(), url, 1*time.Hour); err != nil {
		panic(err)
	}
	return match, nil
}
