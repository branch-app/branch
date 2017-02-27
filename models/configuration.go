package models

// Configuration contains the structure of the service's configuration.
type Configuration struct {
	SentryDSN string `json:"sentryDSN"`
	Port      string `json:"port"`
}
