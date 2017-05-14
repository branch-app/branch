branch-mono-go
===

This monorepo contains all of Branch App's Golang code.

#### Clients

- [Auth](/clients/auth) - This client contains functions for interacting with the auth
service.
- [XboxLive](/clients/xboxlive) - This client contains functions for interacting with the
xbox live service. Mainly for identity management.

#### Services
Please visit each service's own README to view api specifics.

- [Halo 4](/services/halo4) - This service contains all the logic for interacting with
the external Halo Waypoint API. It also contains logic for processing all of the cached
statistic data.
- [Xbox Live](/services/xboxlive) - This service contains all the logic for interacting
with the external Xbox Live API. It also contains logic for processing all of the cached
statistic data.


#### Libraries

- [ConfigLoader](/libraries/configloader) - This library handles loading a configuration
file from the root of the working directory based on the selected application environment.
- [Crypto](/libraries/crypto) - This library contains functions for executing commonly
used cryptographic operations.
- [JsonClient](/libraries/jsonclient) - This library contains a simple http wrapper for
communicating over HTTP exclusively in JSON.
- [JsonSchemaClient](/libraries/jsonschemaclient) - This library handles json schema
validation in all the services. It loads and parses all schemas into memory on application
boot to speed up request validation.
- [Log](/libraries/log) - This library handles all internal logging.
- [Mongo](/libraries/mongo) - This library contains functions for simplifying
communication with a mongodb database.
- [Routing](/libraries/routing) - This library contains essentially `middleware` that
every service uses to handle it's routing from `/:v/:version/:method` to the relevant
golang function. This includes handling data input/output.
