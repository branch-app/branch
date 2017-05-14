package models

// Config defines the structure of the services configuration
type Config struct {
	Addr  string `json:"addr"`
	Mongo string `json:"mongo"`

	AuthURL     string `json:"authUrl"`
	XboxLiveURL string `json:"xboxliveUrl"`

	AuthKey     string `json:"authKey"`
	XboxLiveKey string `json:"xboxliveKey"`
}
