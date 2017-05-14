package auth

import "time"

// Halo4Token defines the structure of the response from the auth service when requesting
// a halo 4 auth token.
type Halo4Token struct {
	SpartanToken   string    `json:"spartanToken"`
	Gamertag       string    `json:"gamertag"`
	AnalyticsToken string    `json:"analyticsToken"`
	ExpiresAt      time.Time `json:"expiresAt"`
}
