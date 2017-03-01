package models

import "time"

// XboxLiveIdentity holds a user's Gamertag and XUID.
type XboxLiveIdentity struct {
	// CachedAt is when the information was retrieved from the Xbox Live servers.
	CachedAt time.Time `json:"cached_at"`

	// ExpiresAt is the UTC time that the Identity expires.
	ExpiresAt time.Time `json:"expires_at"`

	// Gamertag is the Xbox Live User's gamertag.
	Gamertag string `json:"gamertag"`

	// Gamertag is the Xbox Live User's Xbox Universal Identifier.
	XUID string `json:"xuid"`
}

// Fresh checks to see if the identity hasn't expired and can be used.
func (identity *XboxLiveIdentity) Fresh() bool {
	return identity.ExpiresAt.After(time.Now().UTC())
}

// NewXboxLiveIdentity creates a new XboxLiveIdentity based on a Gamertag and XUID.
func NewXboxLiveIdentity(gamertag, xuid string, cachedAt time.Time) *XboxLiveIdentity {
	return &XboxLiveIdentity{
		CachedAt:  cachedAt,
		ExpiresAt: cachedAt.Add(25 * time.Minute),
		Gamertag:  gamertag,
		XUID:      xuid,
	}
}
