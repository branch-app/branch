package clients

import (
	"time"

	"fmt"

	"github.com/branch-app/service-xboxlive/models"
	"github.com/branch-app/service-xboxlive/models/xboxlive"
	"github.com/branch-app/shared-go/models/branch"
	"github.com/branch-app/shared-go/crypto"
	sharedModels "github.com/branch-app/shared-go/models"
	"gopkg.in/mgo.v2/bson"
)

const (
	profileSettingsURL = "https://profile.xboxlive.com/users/%s(%s)/profile/settings?settings=gamertag"
	profileURL         = "https://profile.xboxlive.com/users/xuid(%s)/profile/settings?settings=GameDisplayPicRaw,Gamerscore,Gamertag,AccountTier,XboxOneRep,PreferredColor,RealName,Bio,TenureLevel,Watermarks,Location,ShowUserAsAvatar"
)

func (client *XboxLiveClient) GetProfileIdentity(identityCall *sharedModels.IdentityCall) (*models.XboxLiveIdentity, error) {
	// Check mem cache
	var identity *models.XboxLiveIdentity
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
	identity = models.NewXboxLiveIdentity(gamertag, xuid, time.Now().UTC())

	// Add to mem cache and return
	client.xblStore.Set(identity)
	return identity, nil
}

func (client *XboxLiveClient) GetProfileSettings(identity *models.XboxLiveIdentity) (*branch.Response, error) {
	url := fmt.Sprintf(profileURL, identity.XUID)
	urlHash := crypto.CreateSHA512Hash(url)
	auth, authErr := client.GetAuthentication()

	// Check if we have a cached document
	cacheRecord, err := sharedModels.CacheRecordFindOne(client.mongoClient, bson.M{"doc_url_hash": urlHash})
	if err != nil {
		return nil, err
	}
	if cacheRecord != nil && (authErr != nil || cacheRecord.IsValid()) {
		response, err := xboxlive.ProfileUsersFindOne(client.mongoClient, bson.M{"_id": cacheRecord.DocumentID})
		if err != nil {
			return nil, err
		}
		return branch.NewResponse(cacheRecord.CachedAt, &response), nil
	}
	if authErr != nil {
		return nil, client.handleError(nil, authErr)
	}

	// Retrieve data from Xbox Live
	var profileUsers *xboxlive.ProfileUsers
	_, err = client.ExecuteRequest("GET", url, auth, 3, nil, &profileUsers)
	if err != nil {
		profileUsers, _ = xboxlive.ProfileUsersFindOne(client.mongoClient, bson.M{"_id": cacheRecord.DocumentID})
		if profileUsers != nil {
			return branch.NewResponse(cacheRecord.CachedAt, &profileUsers), nil
		}
		return nil, client.handleError(&profileUsers.Response, err)
	}

	// Deal with caching
	if cacheRecord != nil {
		// Already have a cache record - so just update
		profileUsers.Id = cacheRecord.DocumentID
		profileUsers.Save(client.mongoClient)

		// Update Cache Record
		cacheRecord.Update(client.mongoClient)
	} else {
		// Need to add to database
		profileUsers.Save(client.mongoClient)
		cacheRecord = sharedModels.NewCacheRecord(url, profileUsers.Id, 5*time.Minute)
		cacheRecord.Save(client.mongoClient)
	}

	return branch.NewResponse(cacheRecord.CachedAt, profileUsers), nil
}
