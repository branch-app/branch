package xboxlive

import (
	"fmt"
	"net/url"
	"strconv"

	authClient "github.com/branch-app/branch-mono-go/clients/auth"
	"github.com/branch-app/branch-mono-go/domain/auth"
	"github.com/branch-app/branch-mono-go/libraries/crypto"
	"github.com/branch-app/branch-mono-go/libraries/jsonclient"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/mongo"
	"github.com/branch-app/branch-mono-go/services/xboxlive/stores"
)

type Client struct {
	profileClient *jsonclient.Client
	authClient    *authClient.Client

	mongoDb *mongo.Client

	identityStore  *stores.Identity
	authentication auth.XboxLiveToken
}

func (client *Client) Do(jsonClient *jsonclient.Client, method, endpoint, collection string, query jsonclient.M, body, response interface{}, contractVer int) (bool, *log.E) {
	// Construct URL and hash
	_, hash := constructURL(jsonClient, endpoint)

	// Check for cache copy
	cacheResp, err := client.mongoDb.GetDocumentCacheInformation(collection, hash)
	if err != nil && err.Code != "document_not_found" {
		return false, err
	}

	// Check if cache copy is valid
	if cacheResp != nil && cacheResp.CacheInformation.IsValid() {
		e := client.mongoDb.FindByID(cacheResp.ID, collection, response)
		return true, e
	}

	// Get Xbox Live Auth Token, and query data from the Xbox Live API
	xblToken, err := client.authClient.GetXboxLiveToken()
	if err != nil {
		return false, err
	}
	err = jsonClient.Do(method, endpoint, body, &response, &jsonclient.DoOptions{
		Headers: jsonclient.M{
			"x-xbl-contract-version": strconv.Itoa(contractVer),
			"Authorization":          fmt.Sprintf("XBL3.0 x=%s;%s", xblToken.UserHash, xblToken.Token),
		},
		Query: query,
	})

	// If there was an error, try to return cached copy - else return error
	if err == nil {
		return false, nil
	}
	if cacheResp != nil {
		return true, client.mongoDb.FindByID(cacheResp.ID, collection, response)
	}
	return false, err
}

func constructURL(jsClient *jsonclient.Client, endpoint string) (string, string) {
	base, err := url.Parse(jsClient.BaseURL())
	endpnt, err := url.Parse(endpoint)
	if err != nil {
		panic(err)
	}

	resolved := base.ResolveReference(endpnt)
	resolvedStr := resolved.String()
	return resolvedStr, crypto.CreateSHA512Hash(resolvedStr)
}

// NewClient creates a new Xbox Live and initiates authentication.
func NewClient(authClient *authClient.Client, mongoConnectionStr, mongoDbName string) *Client {
	return &Client{
		profileClient: jsonclient.NewClient("https://profile.xboxlive.com/", nil),
		authClient:    authClient,

		mongoDb:       mongo.NewClient(mongoConnectionStr, mongoDbName, true),
		identityStore: stores.NewIdentityStore(),
	}
}
