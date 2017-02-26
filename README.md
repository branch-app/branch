service-auth
===

This service handles dealing with the various authentication providers used by the
platform.

### Documentation

#### `GET` v1/halo-4

``` json
{
	"spartan_token": "v2=token",
	"gamertag": "Example App",
	"analytics_token":"3E928F6938EB285FD760C382EBD967ABD275C93572425948EB1694186FEF7AAF"
}
```

#### `GET` v1/xbox-live

``` json
{
  "access_token": "token",
  "token_type": "bearer",
  "expires_in": "86400",
  "scope": "service::user.auth.xboxlive.com::MBI_SSL",
  "refresh_token": "refresh_token",
  "user_id": "aaa9983fdd3888e2"
}
```

