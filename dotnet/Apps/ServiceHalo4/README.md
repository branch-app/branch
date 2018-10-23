# service-halo4

This service handles communication, transformation, and caching of Halo 4 stats from the Halo Waypoint stats API. All player stats are cached against the XUID of the player, so if they change gamertag, we don't have to re-cache any content 🤷🏻‍.

Base URL: `/1`  
Example URL: `https://service-halo4.branch-app.co/1`

## Configuration

``` json
{
  "SentryDSN": "<optional sentry DSN>",
  "Services": {
    "Auth": {
      "Url": "http://service-auth",
      "Key": "<auth service key>"
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

Retrieves the Halo 4 Service Record of the player. *10 minute cache.*

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

### `get_recent_matches`

Retrieves a list of recent Halo 4 matches for the player. *10 minute cache.*

#### Request
```json
{
  "identity": {
    "type": "gamertag",
    "value": "PhoenixBanTrain"
  },
  "game_mode": "war-games",
  "start_at": 15,
  "count": 5
}
```

The `game_mode` field is an enum of either `war-games`, `campaign`, `spartan-ops`, or `custom-games`. The `start_at` and `count` fields are optional. Start at must be an integer greater than 0, and count must be an integer greater than 1 and equal to or less than 50.

#### Request
```json
{
  "cache_info": {
    "cached_at": "2018-09-01T13:48:16.67Z",
    "expires_at": "2018-09-01T14:03:16.67Z"
  },
  "matches": [...],
  "has_more_matches": true,
  "date_fidelity": "rough"
}
```
