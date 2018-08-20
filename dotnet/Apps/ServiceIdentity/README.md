# service-identity

This service handles dealing with the identity providers that branch uses. Currently this is only Xbox Live.

Base URL: `/1`  
Example URL: `https://service-identity.branch-app.co/1`

## Configuration

``` json
{
  "sentry_dsn": "<optional sentry DSN>",
  "services": {
    "service-auth": {
      "url": "http://service-auth",
      "key": "<auth service key>"
    }
  },
  "key": "<secret key used when service is called>"
}
```

## Versions

- `2018-07-30`: service created

## Methods

### `get_xbox_live_identity`

Retrieves an identity profile based on the provided `gamertag` or `xuid`. Results are cached for 15 minutes.

#### Request
```json
{
  "type": "xuid|gamertag",
  "value": "Program"
}
```

#### Response
```json
{
  "cache": {
    "id": "cache_000000BTzg1qjTphUeVmlRWFufWAy",
    "cached_at": "2018-08-01T19:00:23Z",
    "expires_at": "2018-08-01T19:15:23Z"
  },
  "xuid": "9709775544905632",
  "gamertag": "Program"
}
```
