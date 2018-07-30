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

