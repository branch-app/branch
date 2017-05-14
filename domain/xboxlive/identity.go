package xboxlive

import (
	"time"

	"fmt"

	"github.com/branch-app/branch-mono-go/domain/branch"
)

// XboxLiveIdentity holds a user's Gamertag and XUID.
type Identity struct {
	branch.Response

	// Gamertag is the Xbox Live User's gamertag.
	Gamertag string `json:"gamertag"`

	// Gamertag is the Xbox Live User's Xbox Universal Identifier.
	XUID string `json:"xuid"`
}

// IsValid checks to see if the identity hasn't expired and can be used.
func (identity *Identity) IsValid() bool {
	return identity.CacheInformation.ExpiresAt.After(time.Now().UTC())
}

// NewIdentity creates a new Identity based on a Gamertag and XUID.
func NewIdentity(gamertag, xuid string, cachedAt time.Time) *Identity {
	url := fmt.Sprintf("branch://identity/xuid:%s-gamertag:%s", xuid, gamertag)

	return &Identity{
		Gamertag: gamertag,
		XUID:     xuid,
		Response: branch.NewResponse(url, cachedAt, 25*time.Minute),
	}
}
