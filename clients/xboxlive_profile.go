package clients

import (
	"time"

	"fmt"

	"github.com/branch-app/service-xboxlive/models"
	"github.com/branch-app/service-xboxlive/models/branch"
	"github.com/branch-app/service-xboxlive/models/xboxlive"
	sharedHelpers "github.com/branch-app/shared-go/helpers"
	"gopkg.in/mgo.v2/bson"
)

const (
	profileSettingsURL = "https://profile.xboxlive.com/users/%s(%s)/profile/settings?settings=gamertag"
	profileURL         = "https://profile.xboxlive.com/users/xuid(%s)/profile/settings?settings=GameDisplayPicRaw,Gamerscore,Gamertag,AccountTier,XboxOneRep,PreferredColor,RealName,Bio,TenureLevel,Watermarks,Location,ShowUserAsAvatar"
)

func (client *XboxLiveClient) GetProfileIdentity(identityCall *models.IdentityCall) (*models.XboxLiveIdentity, error) {
	// Check mem cache
	var identity *models.XboxLiveIdentity
	if identityCall.Type == "xuid" {
		identity = client.xblStore.GetByXUID(identityCall.Identity)
	} else if identityCall.Type == "gamertag" {
		identity = client.xblStore.GetByGT(identityCall.Identity)
	}

	// We still have a fresh identity, return it
	if identity != nil && identity.Fresh() {
		return identity, nil
	}

	// Retrieve info
	url := fmt.Sprintf(profileSettingsURL, identityCall.Type, identityCall.Identity)
	var response xboxlive.ProfileUsers
	_, err := client.ExecuteRequest("GET", url, true, 2, nil, &response)
	if err != nil {
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
	urlHash := sharedHelpers.CreateSHA512Hash(url)

	// Check if we have a cached document
	cacheRecord, err := xboxlive.CacheRecordFindOne(client.mongoClient, bson.M{"doc_url_hash": urlHash})
	if err != nil {
		return nil, err
	}
	if cacheRecord != nil && cacheRecord.IsValid() {
		response, err := xboxlive.ProfileUsersFindOne(client.mongoClient, bson.M{"_id": cacheRecord.DocumentID})
		if err != nil {
			return nil, err
		}
		return branch.NewResponse(cacheRecord.CachedAt, &response), nil
	}

	// Retrieve data from Xbox Live
	var profileUsers *xboxlive.ProfileUsers
	_, err = client.ExecuteRequest("GET", url, true, 3, nil, &profileUsers)
	if err != nil {
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
		cacheRecord = xboxlive.NewCacheRecord(url, profileUsers.Id, 5*time.Minute)
		cacheRecord.Save(client.mongoClient)
	}

	return branch.NewResponse(cacheRecord.CachedAt, profileUsers), nil
}
