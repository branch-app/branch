package models

// Configuration contains the structure of the service's configuration.
type Configuration struct {
	SentryDSN string         `json:"sentryDSN" env:"SENTRY_DSN"`
	Port      string         `json:"port" env:"PORT"`
	MongoDB   *MongoDBConfig `json:"mongoDb" env:"MONGO"`
}

// MongoDBConfig contains the structure of the a MongoDB configuration.
type MongoDBConfig struct {
	ConnectionString string `json:"connectionString" env:"CONNECTION_STRING"`
	DatabaseName     string `json:"databaseName" env:"DATABASE_NAME"`
}
