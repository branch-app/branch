package xboxlive

import (
	"fmt"
	"time"

	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/branch-app/branch-mono-go/libraries/jsonclient"
	"github.com/branch-app/branch-mono-go/libraries/log"
)

type Client struct {
	jsonClient *jsonclient.Client
}

// NewClient creates a new xboxlive service client.
func NewClient(baseURL, key string) *Client {
	options := &jsonclient.ClientOptions{
		Headers: map[string]string{
			"Authorization": fmt.Sprintf("bearer %s", key),
			"Accept":        "application/json",
		},
		Timeout: 30 * time.Second,
	}

	return &Client{
		jsonClient: jsonclient.NewClient(baseURL, options),
	}
}

// GetIdentity gets an Xbox Live Identity based on either a specified gamertag or xuid.
func (client *Client) GetIdentity(identityLookup xboxlive.IdentityLookup) (*xboxlive.Identity, *log.E) {
	var identity *xboxlive.Identity
	err := client.jsonClient.Do("POST", "/1/2017-05-15/get_identity", identityLookup, &identity, nil)
	return identity, err
}
