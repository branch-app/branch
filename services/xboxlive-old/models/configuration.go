package models

// Configuration contains the structure of the service's configuration.
type Configuration struct {
	SentryDSN string             `json:"sentryDSN" env:"SENTRY_DSN"`
	Port      string             `json:"port" env:"PORT"`
	Mongo     MongoConfiguration `json:"mongo" env:"MONGO"`
}

type MongoConfiguration struct {
	Host     string `json:"host" env:"HOST"`
	Database string `json:"database" env:"DATABASE"`
	Username string `json:"username" env:"USERNAME"`
	Password string `json:"password" env:"PASSWORD"`
}
