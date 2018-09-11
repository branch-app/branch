# service-identity

This service handles dealing with the identity providers that branch uses. Currently this is only Xbox Live.

Base URL: `/1`  
Example URL: `https://service-identity.branch-app.co/1`

## Configuration

``` json
{
  "SentryDSN": "<optional sentry DSN>",
  "Services": {
    "Auth": {
      "Url": "http://service-auth",
      "Key": "<auth service key>",
      "Options": { "Timeout": 7000 }
    }
  },
  "InternalKey": "<secret key>"
}
```

## Versions

- `2018-08-19`: service created

## Methods

### `get_xboxlive_identity`

Retrieves an identity profile based on the provided `gamertag` or `xuid`. Results are cached for 15 minutes.

#### Request
```json
{
  "type": "xuid|gamertag",
  "value": "xxcollateralx"
}
```

#### Response
```json
{
  "cache_info": {
    "cached_at": "2018-09-01T13:48:16.67Z",
    "expires_at": "2018-09-01T14:03:16.67Z"
  },
  "gamertag": "xxCoLLaTeRaLx",
  "xuid": 2533274824126595
}
```
