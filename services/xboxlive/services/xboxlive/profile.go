package xboxlive

import (
	"fmt"

	"time"

	"github.com/branch-app/branch-mono-go/domain/branch"
	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/services/xboxlive/models/response"
)

const (
	profileCollection  = "profile_users"
	profileSettingsURL = "users/%s(%s)/profile/settings?settings=gamertag"
	profileURL         = "users/%s(%s)/profile/settings?settings=GameDisplayPicRaw,Gamerscore,Gamertag,AccountTier,XboxOneRep,PreferredColor,RealName,Bio,TenureLevel,Watermarks,Location,ShowUserAsAvatar"
)

// GetIdentity creates a Branch XboxLiveIdentity from a gamertag or xuid.
func (client *Client) GetIdentity(value, valType string) (*xboxlive.Identity, *log.E) {
	// Get identity from store
	var identity *xboxlive.Identity
	if valType == "gamertag" {
		identity = client.identityStore.GetByGamertag(value)
	} else if valType == "gamertag" {
		identity = client.identityStore.GetByXUID(value)
	}

	// If valid, return it now
	if identity != nil && identity.IsValid() {
		return identity, nil
	}

	// Get from xbox live
	endpoint := fmt.Sprintf(profileURL, valType, value)
	url, hash := constructURL(client.profileClient, endpoint)
	var profileUsers *response.ProfileUsers
	cached, err := client.Do(client.profileClient, "GET", endpoint, profileCollection, nil, nil, &profileUsers, 2)
	if err != nil {
		return identity, err
	}

	// Setup Identity
	settings := profileUsers.Users[0].Settings
	xuid := profileUsers.Users[0].XUID
	var gamertag string
	for _, s := range settings {
		if s.ID == "Gamertag" {
			gamertag = s.Value
		}
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		profileUsers.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), 5*time.Minute)
	}

	// Create identity, then upsert into cache in background
	identity = xboxlive.NewIdentity(gamertag, xuid, profileUsers.CacheInformation.CachedAt)
	go client.mongoDb.UpsertByCacheInfoHash(hash, profileUsers, profileCollection)

	// Return identity
	return identity, nil
}
