package models

import "time"

// XboxLiveIdentity holds a user's Gamertag and XUID.
type XboxLiveIdentity struct {
	// Age is when the information was retrieved from the Xbox Live servers.
	Age time.Time

	// ExpiresAt is the UTC time that the Identity expires.
	ExpiresAt time.Time

	// Gamertag is the Xbox Live User's gamertag.
	Gamertag string

	// Gamertag is the Xbox Live User's Xbox Universal Identifier.
	XUID string
}

// NewXboxLiveIdentity creates a new XboxLiveIdentity based on a Gamertag and XUID.
func NewXboxLiveIdentity(gamertag, xuid string, age time.Time) *XboxLiveIdentity {
	return &XboxLiveIdentity{
		Age:       age,
		ExpiresAt: age.Add(25 * time.Minute),
		Gamertag:  gamertag,
		XUID:      xuid,
	}
}
