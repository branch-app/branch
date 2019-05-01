# service-halo2

This service handles dealing with the one-time crawling and caching of the old Halo 2 stats site run by Bungie.

Base URL: `/1`
Example URL: `https://service-halo2.branch-app.co/1`

## Configuration

``` json
{
  "SentryDSN": "<optional sentry DSN>",
  "Services": {},
  "SQS": {
    "AccessKeyId": "",
    "SecretAccessKey": "",
    "ServiceUrl": "https://sqs.eu-west-2.amazonaws.com/..."
  },
  "InternalKey": "<secret key>"
}
```

## Versions

- `2019-04-20`: service created

## Caching

Halo 2 has been shut down, so it isn't possible for new stats to be recorded. This means that caches are immutable and never fetched again. To achieve optimum performance and error handling, cache requests is queued up and done [asynchronously](https://uk.linkedin.com/in/andytomlinson) in the background.

When a request is made to `get_service_record`, `get_match`, or `get_recent_matches`, one of three things will happen:

- Fetch the requested data and return it
- Return the caching status
- The request is queued to be cached and an error stating this is returned.

### Cache progress

Once a request has been queued for caching, this is the error that will be returned to the client:
```json
{
  "code": "request_queued_for_cache"
}
```

If requesting data that is currently being cached, this is the error that will be returned to the client:
```json
{
  "code": "currently_caching"
}
```

If requesting data that failed to be cached correctly it will return an error. In the reasons array will be a list of the errors that cached the caching failure, this is the error that will be returned to the client:
```json
{
  "code": "caching_failure",
  "reasons": [
    {
      "code": "unexpected_page_format",
      "meta": {
        "expected_violator": "Total Length field"
      }
    }
  ]
}
```

## Gamertag replacement

Halo 2 shutdown at midnight on April 15th, 2010. In the 8 years since then I'm sure many many players will have changed gamertags, or even back in the day have had more than 1 due to getting banned or whatever. Due to this, the service has a system for "gamertag replacement" to effectively merge stats together to create a more unified experience.

All the data is maintained, but each endpoint will return an array of the replacements so the client can indicate which accounts were merged.

## Methods

### `get_service_record`

Retrieves the cached service record for the player.

#### Request
```json
{
  "type": "xuid|gamertag",
  "value": "xxcollateralx"
}
```

#### Response
```js
{
  "cache_info": {
    "cached_at": "2018-09-01T13:48:16.67Z"
  },
  ...info
}
```

### `get_match`

### `get_recent_matches`

### `queue_service_record`

### `queue_match_history`

### `queue_match`

## Internal Data

todo

## Queue Events

### Service Record Queue

```json
{
  "code": "cache_service_record",
  "params": {
    "gamertag": "PhoenixBanTrain"
  }
}
```

### Match History Queue

```json
{
  "code": "cache_match_history",
  "params": {
    "gamertag": "PhoenixBanTrain",
    "page": 0
  }
}
```

### Match Queue

```json
{
  "code": "cache_match",
  "params": {
    "match_id": "1234567"
  }
}
```
