package stores

import (
	"time"

	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	slug "github.com/metal3d/go-slugify"
	"github.com/patrickmn/go-cache"
)

// Identity is a in-memory store that handles lookup for Xbox Live identities.
type Identity struct {
	// GamertagStore holds a threadsafe memcache of Gamertag->XUID keyvalues
	GamertagStore *cache.Cache

	// XUIDStore holds a threadsafe memcache of XUID->Gamertag keyvalues
	XUIDStore *cache.Cache
}

// GetByGamertag retrieves an Identity by it's Gamertag
func (store Identity) GetByGamertag(gamertag string) *xboxlive.Identity {
	gtSlug := slug.Marshal(gamertag, true)
	value, exists := store.GamertagStore.Get(gtSlug)
	if !exists {
		return nil
	}

	// Retrieve XboxLiveIdentity from cache
	memIdent := value.(*xboxlive.Identity)
	identity := xboxlive.NewIdentity(memIdent.Gamertag, memIdent.XUID, memIdent.CacheInformation.CachedAt)

	// Update Cache
	store.Set(identity)

	// Return Identity
	return identity
}

// GetByXUID retrives an XboxLiveIdentity by it's XUID.
func (store Identity) GetByXUID(xuid string) *xboxlive.Identity {
	xuidCache := slug.Marshal(xuid, true)
	value, exists := store.XUIDStore.Get(xuidCache)

	if !exists {
		return nil
	}

	// Retrieve XboxLiveIdentity from cache, and return
	memIdent := value.(*xboxlive.Identity)
	identity := xboxlive.NewIdentity(memIdent.Gamertag, memIdent.XUID, memIdent.CacheInformation.CachedAt)

	// Update Opposite Cache
	store.Set(identity)

	// Return Identity
	return identity
}

// Set sets a Gamertag key to an Identity value in the GTStore.
func (store Identity) Set(identity *xboxlive.Identity) {
	gamertagSlug := slug.Marshal(identity.Gamertag, true)
	xuidSlug := slug.Marshal(identity.XUID, true)
	untilExpires := time.Now().UTC().Sub(identity.CacheInformation.CachedAt)
	store.GamertagStore.Set(gamertagSlug, identity, untilExpires)
	store.XUIDStore.Set(xuidSlug, identity, untilExpires)
}

// NewIdentityStore creates a new Xbox Live Identity, and initializes it's inner caches.
func NewIdentityStore() *Identity {
	return &Identity{
		GamertagStore: cache.New(1*time.Hour, 30*time.Second),
		XUIDStore:     cache.New(1*time.Hour, 30*time.Second),
	}
}
