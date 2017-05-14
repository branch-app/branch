service-auth
===

This service handles dealing with the various authentication providers used by the
platform.

## Configuration

To configure this service, you can either populate the config.{environment}.json file (ie;
config.development.json) in the working directory of the service. Or you can set an
environment variable CONFIG that contains the json content of the configuration. Below is
an example json blob.

``` json
{
	"halo4": {
		"account": "me@outlook.com",
		"password": "wQpk2ZLGPsKxDav1nnkneIfJ9q5Hv79zRgMq74dqYfehOV7H84CBa3dBcec8qt0F"
	},
	"xboxLive": {
		"account": "me@outlook.com",
		"password": "wQpk2ZLGPsKxDav1nnkneIfJ9q5Hv79zRgMq74dqYfehOV7H84CBa3dBcec8qt0F"
	}
}

```

## API Documentation

#### `1/2015-05-13/get_halo4_token`

``` json
{
  "spartanToken": "v2=KTydZhZ26o1akkEtdf8cpTwePGTz30kYMe20cAArcyz2bB6db1uR9nUygFXmfGWMomx2bNUDqhei35p4cZjc6IglzmqzT52BFrMDCYd0Rq4o2Xkj4JjXjyQ3pdhthjLxvMR2ArlHUnVTP04ja7ezFeYhL8PN7hAcjgIOTTpsq2Sly6R3gBkKUhLi7XHKNetAtxNfqbjXLI5wSQRsniwPv4BRabwLRJY6c1nOvvkEOhHBEl2gOP2DLst0nvATNSzZOqY0RCm9LiQ9ofAAztnddryOAmactjSVwj2AJ1aoaStALg3i1I913eVIbE3wjHE5eD4oVou6DzfMxP6RrRdvDMY4Xdh8CZUpkyYFiT5vzhAYxHKtHmT5UbHMeMxntr3ABEsVfYxm6IEVPZgl39eLLy1JSRUImtWJ7y6aLQY",
  "gamertag": "Program",
  "analyticsToken": "LAMGNPCJBXCIMGDBK3IX5FW5WOBMM3RXVLKP6JIJAKLG8REOGKSSQZOVYGWMHJI5",
  "expiresAt": "2017-05-13T12:00:00Z"
}
```

#### `1/2015-05-13/get_xboxlive_token`

``` json
{
  "token": "jwt:header.payload.signature",
  "expiresAt": "2017-05-13T12:00:00.00000000",
  "agg": "Adult",
  "gtg": "Program",
  "prv": "data",
  "usr": "userid",
  "xid": "7365763576378564",
  "uhs": "946826504543749674793"
}
```

