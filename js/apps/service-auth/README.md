# service-auth

This service handles dealing with the various authentication providers used by the platform.

## Configuration

To configure this service, you can either populate the config.{environment}.json file (ie; config.development.json) in the working directory of the service. Or you can set an environment variable CONFIG that contains the json content of the configuration. Below is an example json blob.

``` json
{
    "halo4": {
        "account": "me@outlook.com",
        "password": "wQpk2ZLGPsKxDav1nnkneIfJ9q5Hv79zRgMq74dqYfehOV7H84CBa3dBcec8qt0F"
    },
    "xboxlive": {
        "account": "me@outlook.com",
        "password": "wQpk2ZLGPsKxDav1nnkneIfJ9q5Hv79zRgMq74dqYfehOV7H84CBa3dBcec8qt0F"
    },
    "mongodb": "",
    "hashingSalt": "secure_hashing_salt"
}

```

## API Documentation

#### `1/2017-06-25/get_halo4_token`

``` json
{
    "accountHash": "021e2e926461ae621b815805a55975456f23610685ff71a2e8ff4b5b91cf4b5a3b45aee8ee178f21b3a67773933a9678e9b9e89269246b6568baa6a21a678c68",
    "createdAt": "2017-06-25T13:16:15.205Z",
    "expiresAt": "2017-06-25T14:11:15.203Z",
    "spartanToken": "v2=gnBqcakEXSBoPokJLBp9lm8BKx30lp45ad6Om8JyHZCRihLWxwA9qdhfBJPZixzUVVIMHLbw1jwmtiaW9ho3KorHhgtTVeTYheVbM1xhjlUWDLG5UCDV37UJUEodITlp9uAltMsFVkK700VSJVPtMlGIvnrIWRfumTu2NUl8YtMllNb20uaBwYrS44kg8BRGOSHfF6EyGsrlUvwrwKcNgLWnK7SU9GjV5gdjrncQjL9Rvx2xaii2ZGEhhD65PKVPIeMmSfV0NtfCYEsPrh8EzdnVtCA5WC6OStjWIqdrylyyvVkct3oBPgpOKoOAdyjbMny8MsqBE6vieBgfEmOrA9lge8KskW8j65Cz3n7ZKOTaDcBgT0tAWFQ3CGqy6bOgBR6BYguv6MtQfxdLXO9V8XTSQisbrfk3oTuKAnZ",
    "gamertag": "Example Gamertag",
    "analyticsToken": "E24B97ECBE01745E445F0C59EC547BDC757853A2FE1E8D2FFD5849EC6CFC6220",
    "id": "a264fb1a-a9d6-4aac-aa94-56ddb46a5608"
}
```

#### `1/2017-06-25/get_xboxlive_token`

``` json
{
    "accountHash": "021e2e926461ae621b815805a55975456f23610685ff71a2e8ff4b5b91cf4b5a3b45aee8ee178f21b3a67773933a9678e9b9e89269246b6568baa6a21a678c68",
    "createdAt": "2017-06-25T13:25:08.497Z",
    "expiresAt": "2017-06-26T05:25:06.315Z",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ",
    "agg": "Adult",
    "gtg": "Example Gamertag",
    "prv": "379 955 625 281 679 837 446 804 138 499 817 801 092 264 487 959 701 621 728 566 254 798 349 547",
    "usr": "873 193",
    "xid": "3796424569032897",
    "uhs": "59874598978365899358",
    "id": "76d30aac-7ef7-46ef-ad9d-37c926dfc715"
}
```
