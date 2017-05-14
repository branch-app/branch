package models

import "time"

type WaypointAuthentication struct {
	SpartanToken   string    `json:"spartan_token"`
	Gamertag       string    `json:"gamertag"`
	AnalyticsToken string    `json:"analytics_token"`
	ExpiresAt      time.Time `json:"expires_at"`
}
