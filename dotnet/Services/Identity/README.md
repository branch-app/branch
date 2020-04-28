# service-identity

BaseURL: https://service-identity.branch.golf/1

## Versions

- `preview`: Service created

## Api

### Index

- `get_xbl_identity`

### Methods

#### `get_xbl_identity`

##### Request

```json
{
	"type": "gamertag",
	"value": "PhoenixBanTrain"
}
```

The `type` key can be either `gamertag`, or `xuid`.

##### Response

```json
{
	"cache_overview": {
		"cached_at": "2020-04-26T23:00:02.168Z",
		"expires_at": "2020-04-26T23:15:13.614Z"
	},
	"gamertag": "xxCoLLaTeRaLx",
	"xuid": 2533274824126595
}
```

