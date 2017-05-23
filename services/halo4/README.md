service-halo4
===

This service handles dealing with Halo4's Waypoint API.


## Configuration

To configure this service, you can either populate the config.{boot}.json file (ie;
config.debug.json) in the working directory of the service. Or you can set an environment
variable CONFIG that contains the json content of the configuration. Below is a table
defining the options of the configuration, as well as an example json blob.

| Name | Description | Example | Optional? | Json Key |
|------|-------------|---------|-------------|----------|
| Address | The address to run the service on | `localhost:3010` | `yes` | `addr` |
| Mongo DB Connection String | The connection string of the mongo database | `mongodb://localhost:27017` | `no` | `mongo` |
| Auth Service URL | The URL of the auth service you wish to interface with | `http://localhost:3000/` | `no` | `authUrl` |
| Xbox Live Service URL | The URL of the xboxlive service you wish to interface with | `http://localhost:3020/` | `no` | `xboxliveUrl` |
| Auth Service Key | The auth key of the auth service you wish to interface with | `supersecret` | `no` | `authKey` |
| Xbox Live Service Key | The auth key of the xboxlive service you wish to interface with | `supersecret` | `no` | `xboxliveKey` |

```
{
	"addr": "http://service-halo4.dev.branchapp.local:3010",
	"mongo": "mongodb://localhost:27017/",
	"authUrl": "http://service-auth.dev.branchapp.local:3000",
	"xboxliveUrl": "http://service-xboxlive.dev.branchapp.local:3020",
	"authKey": "IJaHPJ1T8zFeu4rJDVCqGqRQD7zAVtEzps337L69F1FxFDcB8TtOaxV9gGlw9f6F",
	"xboxliveKey": "ofbYNy7Pz61l8DvSyHjLVwQW9ay1hm2Tt3LwCaPTlkMYt9eL9OiMuadperRpiJ11"
}

```



## API Documentation

soon. just gotta finish the service first fam
