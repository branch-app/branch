package auth

import (
	"fmt"
	"time"

	"github.com/branch-app/branch-mono-go/domain/auth"
	"github.com/branch-app/branch-mono-go/libraries/jsonclient"
	"github.com/branch-app/branch-mono-go/libraries/log"
)

type Client struct {
	jsonClient *jsonclient.Client
}

// NewClient creates a new auth service client.
func NewClient(baseURL, key string) *Client {
	options := &jsonclient.ClientOptions{
		Headers: map[string]string{
			"Authorization": fmt.Sprintf("bearer %s", key),
		},
		Timeout: 30 * time.Second,
	}

	return &Client{
		jsonClient: jsonclient.NewClient(baseURL, options),
	}
}

// GetHalo4Token gets a new authentication token from the auth service that can be
// used with the Halo 4 api.
func (client *Client) GetHalo4Token() (auth.Halo4Token, *log.E) {
	var auth auth.Halo4Token
	err := client.jsonClient.Do("POST", "/1/2017-05-13/get_halo4_token", nil, &auth, nil)
	return auth, err
}

// GetXboxLiveToken gets a new authentication token from the auth service that can be
// used with the Xbox Live api.
func (client *Client) GetXboxLiveToken() (auth.XboxLiveToken, *log.E) {
	var auth auth.XboxLiveToken
	err := client.jsonClient.Do("POST", "/1/2017-05-13/get_xboxlive_token", nil, &auth, nil)
	return auth, err
}
