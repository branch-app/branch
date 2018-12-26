# service-token

This service handles the management of retrieving and storing tokens for the various authentication providers used by the platform.

Base URL: `/1`
Example URL: `https://service-token.branch-app.co/1`

## Configuration

Confirmation of this service depends on the type of boot you are doing.

If you're booting via the `debug` entrypoint, you must populate the `config.development.json` file in the working directory of the service.

Otherwise, if you're booing via the `start` entrypoint, you must set an environment variable `CONFIG` that contains the json representation of the configuration. Below is an example json blob.

``` json
{
  "sentry_dsn": "<optional sentry DSN>",
  "providers": {
    "microsoft_account": {
      "account": "<email address for your microsoft account>",
      "password": "<password for your microsoft account>"
    }
  },
  "redis": {
    "host": "<redis server>",
    "port": 6379,
    "database": 1,
    "password": "<redis password>"
  },
  "remote_chrome_host": "<optional url of the remote chrome host>",
  "internal_keys": ["<secret key used when service is called>"]
}
```

It's important to note that if you're setting the `remote_chrome_host` key, you should omit the protocol, so you'd put `chrome:9222` instead of `http://chrome:9222`.

## Authentication

All endpoints on this service authentication. Requests must have have the header `Authorization: bearer {api-key}`.

## Versions
- 2018-03-21: service created

## Methods

### `get_halo4_token`

Gets a valid Halo 4 spartan token, used to communicate with the Halo 4 API. It will cache the token, and only retrieve a new one every 50-60 minutes.

#### Request
```json
{
  "force_refresh": false
}
```

#### Response
``` json
{
  "spartan_token": "v2=gnBqcakEXSBoPokJLBp9lm8BKx30lp45ad6Om8JyHZCRihLWxwA9qdhfBJPZixzUVVIMHLbw1jwmtiaW9ho3KorHhgtTVeTYheVbM1xhjlUWDLG5UCDV37UJUEodITlp9uAltMsFVkK700VSJVPtMlGIvnrIWRfumTu2NUl8YtMllNb20uaBwYrS44kg8BRGOSHfF6EyGsrlUvwrwKcNgLWnK7SU9GjV5gdjrncQjL9Rvx2xaii2ZGEhhD65PKVPIeMmSfV0NtfCYEsPrh8EzdnVtCA5WC6OStjWIqdrylyyvVkct3oBPgpOKoOAdyjbMny8MsqBE6vieBgfEmOrA9lge8KskW8j65Cz3n7ZKOTaDcBgT0tAWFQ3CGqy6bOgBR6BYguv6MtQfxdLXO9V8XTSQisbrfk3oTuKAnZ",
  "gamertag": "Example Gamertag",
  "analytics_token": "E24B97ECBE01745E445F0C59EC547BDC757853A2FE1E8D2FFD5849EC6CFC6220",
  "expires_at": "2018-03-21T23:11:15.203Z"
}
```

### `get_xboxlive_token`

Gets a valid Xbox Live authentication token, used to communicate with the Xbox Live API. It will cache the token, and only retrieve a new one every 50-60 minutes.

#### Request
```json
{
  "force_refresh": false
}
```

#### Response
``` json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ",
  "agg": "Adult",
  "gtg": "Example Gamertag",
  "prv": "379 955 625 281 679 837 446 804 138 499 817 801 092 264 487 959 701 621 728 566 254 798 349 547",
  "usr": "873 193",
  "xid": "3796424569032897",
  "uhs": "59874598978365899358",
  "expires_at": "2018-03-21T23:25:06.315Z"
}
```
