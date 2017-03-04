package models

// Configuration contains the structure of the service's configuration.
type Configuration struct {
	SentryDSN string         `json:"sentryDSN"`
	Port      string         `json:"port"`
	MongoDB   *MongoDBConfig `json:"mongoDb"`
}

type MongoDBConfig struct {
	ConnectionString string `json:"connectionString"`
	DatabaseName     string `json:"databaseName"`
}
