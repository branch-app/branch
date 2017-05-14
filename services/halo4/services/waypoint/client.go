package waypoint

import (
	"net/url"

	"time"

	authClient "github.com/branch-app/branch-mono-go/clients/auth"
	"github.com/branch-app/branch-mono-go/domain/auth"
	"github.com/branch-app/branch-mono-go/libraries/jsonclient"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/mongo"
	"github.com/branch-app/shared-go/crypto"
)

type Client struct {
	jsonClient *jsonclient.Client
	authClient *authClient.Client

	mongoDb *mongo.Client

	authentication auth.Halo4Token
	// cron           *cron.Cron
}

func (client *Client) Do(method, endpoint, collection string, query jsonclient.M, body, response interface{}, contractVer int) (bool, *log.E) {
	// Construct URL and hash
	_, hash := client.constructURL(endpoint)

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
	waypointToken, err := client.getHalo4Token()
	if err != nil {
		return false, err
	}
	err = client.jsonClient.Do(method, endpoint, body, &response, &jsonclient.DoOptions{
		Headers: jsonclient.M{
			"X-343-Authorization-Spartan": waypointToken.SpartanToken,
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

func (client *Client) getHalo4Token() (auth.Halo4Token, *log.E) {
	if client.authentication.ExpiresAt.After(time.Now().UTC()) {
		return client.authentication, nil
	}

	return client.authClient.GetHalo4Token()
}

func (client *Client) constructURL(endpoint string) (string, string) {
	base, err := url.Parse(client.jsonClient.BaseURL())
	endpnt, err := url.Parse(endpoint)
	if err != nil {
		panic(err)
	}

	resolved := base.ResolveReference(endpnt)
	resolvedStr := resolved.String()
	return resolvedStr, crypto.CreateSHA512Hash(resolvedStr)
}

// NewClient creates a new Halo 4 Client and initiates authentication.
func NewClient(authClient *authClient.Client, mongoConnectionStr, mongoDbName string) *Client {
	client := &Client{
		jsonClient: jsonclient.NewClient("https://stats.svc.halowaypoint.com/", nil),
		authClient: authClient,

		mongoDb: mongo.NewClient(mongoConnectionStr, mongoDbName, true),
	}

	authentication, err := authClient.GetHalo4Token()
	if err != nil {
		panic(err)
	}

	client.authentication = authentication
	return client
}
