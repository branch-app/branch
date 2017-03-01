package models

// Configuration contains the structure of the service's configuration.
type Configuration struct {
	SentryDSN string         `json:"sentryDSN"`
	Port      string         `json:"port"`
	MongoDB   *MongoDBConfig `json:"mongoDb"`
	MySQL     *MySQLConfig   `json:"mysql"`
}

type MongoDBConfig struct {
	ConnectionString string `json:"connectionString"`
	DatabaseName     string `json:"databaseName"`
}

type MySQLConfig struct {
	ConnectionString string `json:"connectionString"`
}
