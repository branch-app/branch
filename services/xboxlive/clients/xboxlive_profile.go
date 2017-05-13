package clients

import (
	"time"

	"fmt"

	"github.com/branch-app/service-xboxlive/models/xboxlive"
	"github.com/branch-app/shared-go/crypto"
	sharedModels "github.com/branch-app/shared-go/models"
)

const (
	profileSettingsURL = "https://profile.xboxlive.com/users/%s(%s)/profile/settings?settings=gamertag"
	profileURL         = "https://profile.xboxlive.com/users/xuid(%s)/profile/settings?settings=GameDisplayPicRaw,Gamerscore,Gamertag,AccountTier,XboxOneRep,PreferredColor,RealName,Bio,TenureLevel,Watermarks,Location,ShowUserAsAvatar"
)

func (client *XboxLiveClient) GetProfileIdentity(identityCall *sharedModels.IdentityCall) (*sharedModels.XboxLiveIdentity, error) {
	// Check mem cache
	var identity *sharedModels.XboxLiveIdentity
	if identityCall.Type == "xuid" {
		identity = client.xblStore.GetByXUID(identityCall.Identity)
	} else if identityCall.Type == "gamertag" {
		identity = client.xblStore.GetByGT(identityCall.Identity)
	}

	// We still have a fresh identity, return it
	auth, authErr := client.GetAuthentication()
	if identity != nil && (authErr != nil || identity.Fresh()) {
		return identity, nil
	}
	if authErr != nil {
		return nil, client.handleError(nil, authErr)
	}

	// Retrieve info
	url := fmt.Sprintf(profileSettingsURL, identityCall.Type, identityCall.Identity)
	var response xboxlive.ProfileUsers
	_, err := client.ExecuteRequest("GET", url, auth, 2, nil, &response)
	if err != nil {
		if identity != nil {
			return identity, nil
		}
		return nil, client.handleError(&response.Response, err)
	}

	// Pull data about profile
	settings := response.Users[0].Settings
	gamertag := settings[0].Value
	xuid := response.Users[0].XUID
	identity = sharedModels.NewXboxLiveIdentity(gamertag, xuid, time.Now().UTC())

	// Add to mem cache and return
	client.xblStore.Set(identity)
	return identity, nil
}

func (client *XboxLiveClient) GetProfileSettings(identity *sharedModels.XboxLiveIdentity) (*xboxlive.ProfileUsers, error) {
	url := fmt.Sprintf(profileURL, identity.XUID)
	urlHash := crypto.CreateSHA512Hash(url)
	auth, authErr := client.GetAuthentication()

	cacheInfo := xboxlive.GetCacheInfo(client.mongoClient.DB(), xboxlive.ProfileUsersCollectionName, urlHash)
	if cacheInfo != nil && (authErr != nil || cacheInfo.CacheInformation.IsValid()) {
		return xboxlive.ProfileUsersFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
	}
	if authErr != nil {
		return nil, client.handleError(nil, authErr)
	}

	// Retrieve data from Xbox Live
	var profileUsers *xboxlive.ProfileUsers
	_, err := client.ExecuteRequest("GET", url, auth, 3, nil, &profileUsers)
	if err != nil {
		if cacheInfo != nil {
			return xboxlive.ProfileUsersFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
		}
		return nil, client.handleError(&profileUsers.Response, err)
	}

	// Preserve ID and CreatedAt if we only updating
	if cacheInfo != nil {
		profileUsers.BranchResponse.ID = cacheInfo.ID
		profileUsers.BranchResponse.CreatedAt = cacheInfo.CreatedAt
	}

	if err := profileUsers.Upsert(client.mongoClient.DB(), url, 5*time.Minute); err != nil {
		panic(err)
	}
	return profileUsers, nil
}
