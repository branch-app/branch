# service-halo4

This service handles communication, transformation, and caching of Halo 4 stats from the Halo Waypoint stats API. All player stats are cached against the XUID of the player, so if they change gamertag, we don't have to re-cache any content 🤷🏻‍.

Base URL: `/1`
Example URL: `https://service-halo4.branch-app.co/1`

## Configuration

``` json
{
  "SentryDSN": "<optional sentry DSN>",
  "Services": {
    "Token": {
      "Url": "http://service-token",
      "Key": "<token service key>"
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

### `get_player_overview`

Retrieves an overview of the Halo 4 Service Record of the player. *10 minute cache.*

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
    "cached_at": "2018-12-30T18:26:25.024942Z",
    "expires_at": "2018-12-30T18:36:25.024942Z"
  },
  "identity": {
    "gamertag": "PhoenixBanTrain",
    "xuid": 2533274956338602,
    "service_tag": "DOPE",
    "emblem_url": "https://emblems.svc.halowaypoint.com/h4/emblems/white_cobalt_grid-on-silver_4diamonds?size={size}"
  },
  "favorite_weapon": {
    "id": 16,
    "name": "Battle Rifle",
    "description": "DESIGNATION: BR85 Heavy-Barrel Service Rifle. The BR85HB Service Rifle is a gas-operated, magazine-fed, semi-automatic rifle optimized for three-round burst firing, and proven to be an extraordinarily versatile weapon at a wide variety of ranges in the hands of a capable marksman.",
    "image_url": "https://assets.halowaypoint.com/games/h4/damage-types/v1/{size}/battle-rifle.png",
    "total_kills": 5686
  },
  "current_rank": {
    "id": 129,
    "name": "130",
    "image_url": "https://assets.halowaypoint.com/games/h4/ranks/v1/{size}/sr-130.png",
    "start_xp": 338400
  },
  "top_medals": [
    {
      "id": 17,
      "name": "Unfriggenbelievable",
      "description": "Kill 40 opponents without dying.",
      "image_url": "https://assets.halowaypoint.com/games/h4/medals/v1/{size}/spree-unfriggenbelievable.png",
      "total_awarded": 41
    }
  ],
  "xp": 338400,
  "spartan_points": 52,
  "total_games_started": 1668,
  "total_medals_earned": 50823,
  "total_gameplay": "P7DT21H24M55S",
  "total_challenges_completed": 165,
  "total_loadout_items_purchased": 39,
  "total_commendation_progress": 0.97
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
