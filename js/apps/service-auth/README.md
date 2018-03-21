# service-auth

This service handles dealing with the various authentication providers used by the platform.

## Configuration

To configure this service, you can either populate the `config.{environment}.json` file (ie; `config.development.json`) in the working directory of the service. Or you can set an environment variable `CONFIG` that contains the json content of the configuration. Below is an example json blob.

``` json
{
    "providers": {
        "microsoft_account": {
            "account": "me@outlook.com",
            "password": "wQpk2ZLGPsKxDav1nnkneIfJ9q5Hv79zRgMq74dqYfehOV7H84CBa3dBcec8qt0F"
        }
    },
    "redis": {
        "host": "redis-server",
        "port": 6379,
        "database": 1,
        "password": "redis-password"
    }
}

```

## API Documentation

All endpoints on this service authentication. Requests must have have the header `Authorization: {api-key}`.

#### `1/2017-06-25/get_halo4_token`

##### Request
``` json
{ }
```

##### Response
``` json
{
    "spartan_token": "v2=gnBqcakEXSBoPokJLBp9lm8BKx30lp45ad6Om8JyHZCRihLWxwA9qdhfBJPZixzUVVIMHLbw1jwmtiaW9ho3KorHhgtTVeTYheVbM1xhjlUWDLG5UCDV37UJUEodITlp9uAltMsFVkK700VSJVPtMlGIvnrIWRfumTu2NUl8YtMllNb20uaBwYrS44kg8BRGOSHfF6EyGsrlUvwrwKcNgLWnK7SU9GjV5gdjrncQjL9Rvx2xaii2ZGEhhD65PKVPIeMmSfV0NtfCYEsPrh8EzdnVtCA5WC6OStjWIqdrylyyvVkct3oBPgpOKoOAdyjbMny8MsqBE6vieBgfEmOrA9lge8KskW8j65Cz3n7ZKOTaDcBgT0tAWFQ3CGqy6bOgBR6BYguv6MtQfxdLXO9V8XTSQisbrfk3oTuKAnZ",
    "gamertag": "Example Gamertag",
    "analytics_token": "E24B97ECBE01745E445F0C59EC547BDC757853A2FE1E8D2FFD5849EC6CFC6220",
    "expires_at": "2017-06-25T14:11:15.203Z"
}
```

#### `1/2017-06-25/get_xboxlive_token`

##### Request
``` json
{ }
```

##### Response
``` json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ",
    "agg": "Adult",
    "gtg": "Example Gamertag",
    "prv": "379 955 625 281 679 837 446 804 138 499 817 801 092 264 487 959 701 621 728 566 254 798 349 547",
    "usr": "873 193",
    "xid": "3796424569032897",
    "uhs": "59874598978365899358",
    "expires_at": "2017-06-26T05:25:06.315Z"
}
```
