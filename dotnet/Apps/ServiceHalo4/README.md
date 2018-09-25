# service-halo4

This service handles communication, transformation, and caching of Halo 4 stats from the Halo Waypoint stats API.

Base URL: `/1`  
Example URL: `https://service-halo4.branch-app.co/1`

## Configuration

``` json
{
  "SentryDSN": "<optional sentry DSN>",
  "Services": {
    "Auth": {
      "Url": "http://service-auth",
      "Key": "<auth service key>",
      "Options": { "Timeout": 7000 }
    },
    "Identity": {
      "Url": "http://service-identity",
      "Key": "<identity service key>"
    }
  },
  "S3": {
    "AccessKeyId": "<s3 access key>",
    "SecretAccessKey": "<s3 secret access key>",
    "Region": "<s3 region>",
    "Bucket": "<s3 bucket>"
  },
  "InternalKey": "<secret key>"
}
```

## Versions

- `2018-09-12`: service created

## Methods

### `get_service_record`

Retrieves the Halo 4 Service Record of the player. It is cached for 10 minutes against their XUID, so if they change gamertag, we don't have to re-cache any content 🤷🏻‍.

#### Request
```json
{
  "identity": {
    "type": "gamertag",
    "value": "PhoenixBanTrain"
  }
}
```

#### Response
```json
{
  "cache_info": {
    "cached_at": "2018-09-01T13:48:16.67Z",
    "expires_at": "2018-09-01T14:03:16.67Z"
  },
  [service_record]
}
```
