package helpers

import (
	"time"

	"github.com/TheTree/service-xboxlive/models"
	slug "github.com/metal3d/go-slugify"
	"github.com/patrickmn/go-cache"
)

// XboxLiveStore is a custom store that handles both Gamertags and XUIDs
type XboxLiveStore struct {
	// GTStore holds a threadsafe memcache of GT->XUID keyvalues
	GTStore *cache.Cache

	// XUIDStore holds a threadsafe memcache of XUID->GT keyvalues
	XUIDStore *cache.Cache
}

// GetByGT retrieves an XboxLiveIdentity by it's Gamertag
func (store XboxLiveStore) GetByGT(gamertag string) *models.XboxLiveIdentity {
	gtSlug := slug.Marshal(gamertag)
	value, exists := store.GTStore.Get(gtSlug)

	if !exists {
		return nil
	}

	// Retrieve XboxLiveIdentity from cache
	memIdent := value.(models.XboxLiveIdentity)
	identity := models.NewXboxLiveIdentity(memIdent.Gamertag, memIdent.XUID, memIdent.Age)

	// Update Opposite Cache
	store.SetXUID(identity.XUID, identity)

	// Return Identity
	return identity
}

// SetGT sets an Gamertag key to an Identity value in the GTStore.
func (store XboxLiveStore) SetGT(gamertag string, identity *models.XboxLiveIdentity) {
	gamertagSlug := slug.Marshal(gamertag)
	untilExpires := time.Now().UTC().Sub(identity.ExpiresAt)
	store.GTStore.Set(gamertagSlug, identity, untilExpires)
}

// GetByXUID retrives an XboxLiveIdentity by it's XUID.
func (store XboxLiveStore) GetByXUID(xuid string) *models.XboxLiveIdentity {
	xuidCache := slug.Marshal(xuid)
	value, exists := store.XUIDStore.Get(xuidCache)

	if !exists {
		return nil
	}

	// Retrieve XboxLiveIdentity from cache, and return
	memIdent := value.(models.XboxLiveIdentity)
	identity := models.NewXboxLiveIdentity(memIdent.Gamertag, memIdent.XUID, memIdent.Age)

	// Update Opposite Cache
	store.SetGT(identity.Gamertag, identity)

	// Return Identity
	return identity
}

// SetXUID sets an XUID key to an Identity value in the XUIDStore.
func (store XboxLiveStore) SetXUID(xuid string, identity *models.XboxLiveIdentity) {
	xuidSlug := slug.Marshal(xuid)
	untilExpires := time.Now().UTC().Sub(identity.ExpiresAt)
	store.XUIDStore.Set(xuidSlug, identity, untilExpires)
}

// NewXboxLiveStore creates a new XboxLiveStore, and initializes their inner sets.
func NewXboxLiveStore() *XboxLiveStore {
	return &XboxLiveStore{
		GTStore:   cache.New(25*time.Minute, 30*time.Second),
		XUIDStore: cache.New(25*time.Minute, 30*time.Second),
	}
}
