# service-xboxlive

This service handles dealing with the identity providers that branch uses. Currently this is only Xbox Live.

Base URL: `/1`
Example URL: `https://service-xboxlive.branch-app.co/1`

## Configuration

``` json
{
  "SentryDSN": "<optional sentry DSN>",
  "Services": {
    "Token": {
      "Url": "http://service-token",
      "Key": "<token service key>",
      "Options": { "Timeout": 7000 }
    }
  },
  "InternalKey": "<secret key>"
}
```

## Versions

- `2018-12-30`: service created

## Methods
