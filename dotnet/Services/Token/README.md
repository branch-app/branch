# service-token

BaseURL: https://service-token.branch.golf/1

## Versions

- `2020-04-27`: Service created

## Api

### Index

- `get_xboxlive_token`

### Methods

#### `get_xbl_token`

##### Request

```json
{
	"ignore_cache": true
}
```

##### Response

```json
{
	"cache_overview": {
		"cached_at": "2020-04-26T23:00:02.168Z",
		"expires_at": "2020-04-26T23:15:13.614Z"
	},
	"token": "blah blah",
	"uhs": "blah"
}
```

