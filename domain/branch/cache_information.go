package branch

import (
	"time"

	"github.com/branch-app/shared-go/crypto"
)

// CacheInformation defines the structure of a Branch Cache Information struct.
type CacheInformation struct {
	DocURLHash string `json:"docUrlHash" bson:"docUrlHash"`

	DocURL string `json:"docUrl" bson:"docUrl"`

	CachedAt time.Time `json:"cachedAt" bson:"cachedAt"`

	ExpiresAt time.Time `json:"expiresAt" bson:"expiresAt"`
}

// IsValid returns if the cache information is still valid.
func (record *CacheInformation) IsValid() bool {
	return record.ExpiresAt.After(time.Now().UTC())
}

// NewCacheInformation creates a CacheInformation struct based on a documents url, cached
// time, and valid duration.
func NewCacheInformation(url string, cachedAt time.Time, validDuration time.Duration) *CacheInformation {
	return &CacheInformation{
		DocURL:     url,
		DocURLHash: crypto.CreateSHA512Hash(url),
		CachedAt:   cachedAt,
		ExpiresAt:  cachedAt.Add(validDuration),
	}
}
