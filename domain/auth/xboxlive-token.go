package auth

import "time"

// XboxLiveToken defines the structure of the response from the auth service when
// requesting an xbox live auth token.
type XboxLiveToken struct {
	Token     string    `json:"token"`
	ExpiresAt time.Time `json:"expiresAt"`
	XUID      string    `json:"xid"`
	Gamertag  string    `json:"gtg"`
	UserHash  string    `json:"uhs"`
}
