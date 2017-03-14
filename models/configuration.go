package models

// Configuration contains the structure of the service's configuration.
type Configuration struct {
	SentryDSN             string `json:"sentryDSN" env:"SENTRY_DSN"`
	Port                  string `json:"port" env:"PORT"`
	MongoConnectionString string `json:"mongoConnectionString" env:"MONGO_CONNECTION_STRING"`
	MongoDatabaseName     string `json:"mongoDatabaseName" env:"MONGO_DATABASE_NAME"`
}
