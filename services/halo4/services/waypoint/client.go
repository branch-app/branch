package waypoint

import (
	"net/url"

	"time"

	"fmt"

	"strings"

	authClient "github.com/branch-app/branch-mono-go/clients/auth"
	"github.com/branch-app/branch-mono-go/domain/auth"
	"github.com/branch-app/branch-mono-go/domain/branch"
	"github.com/branch-app/branch-mono-go/libraries/jsonclient"
	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/mongo"
	"github.com/branch-app/branch-mono-go/services/halo4/models/response"
	"github.com/branch-app/shared-go/crypto"
)

type Client struct {
	statsClient    *jsonclient.Client
	settingsClient *jsonclient.Client
	authClient     *authClient.Client

	mongoDb *mongo.Client

	authentication auth.Halo4Token
	// cron           *cron.Cron
}

const (
	optionsCollection  = "options"
	metadataCollection = "metadata"
	optionsURL         = "RegisterClientService.svc/register/webapp/AE5D20DCFA0347B1BCE0A5253D116752"
	metadataURL        = "h4/metadata?type=%s"
)

const ()

func (client *Client) Do(jsonClient *jsonclient.Client, method, endpoint, collection string, query jsonclient.M, body, response interface{}) (bool, *log.E) {
	// Construct URL and hash
	_, hash := client.constructURL(jsonClient, endpoint)

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
	err = jsonClient.Do(method, endpoint, body, &response, &jsonclient.DoOptions{
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

func (client *Client) constructURL(jsonClient *jsonclient.Client, endpoint string) (string, string) {
	base, err := url.Parse(jsonClient.BaseURL())
	endpnt, err := url.Parse(endpoint)
	if err != nil {
		panic(err)
	}

	resolved := base.ResolveReference(endpnt)
	resolvedStr := resolved.String()
	return resolvedStr, crypto.CreateSHA512Hash(resolvedStr)
}

func (client *Client) GetOptions() (*response.Options, *log.E) {
	// Get from xbox live
	jsonClient := client.settingsClient
	url, hash := client.constructURL(jsonClient, optionsURL)
	var options *response.Options
	cached, err := client.Do(jsonClient, "GET", optionsURL, metadataCollection, nil, nil, &options)
	if err != nil {
		return nil, err
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		options.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), (24*time.Hour)*5)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, options, metadataCollection)

	// Return identity
	return options, nil
}

func (client *Client) GetMetadata(types []string) (*response.Metadata, *log.E) {
	// Get from xbox live
	jsonClient := client.statsClient
	endpoint := fmt.Sprintf(metadataURL, strings.Join(types, ","))
	url, hash := client.constructURL(jsonClient, endpoint)
	var metadata *response.Metadata
	cached, err := client.Do(jsonClient, "GET", endpoint, metadataCollection, nil, nil, &metadata)
	if err != nil {
		return nil, err
	}

	// Set cache data inside profileUsers, if required
	if !cached {
		metadata.CacheInformation = branch.NewCacheInformation(url, time.Now().UTC(), (24*time.Hour)*5)
	}

	// Create identity, then upsert into cache in background
	go client.mongoDb.UpsertByCacheInfoHash(hash, metadata, metadataCollection)

	// Return identity
	return metadata, nil
}

// NewClient creates a new Halo 4 Client and initiates authentication.
func NewClient(authClient *authClient.Client, mongoConnectionStr, mongoDbName string) *Client {
	client := &Client{
		statsClient:    jsonclient.NewClient("https://stats.svc.halowaypoint.com/en-US/", nil),
		settingsClient: jsonclient.NewClient("https://settings.svc.halowaypoint.com/", nil),
		authClient:     authClient,

		mongoDb: mongo.NewClient(mongoConnectionStr, mongoDbName, true),
	}

	authentication, err := authClient.GetHalo4Token()
	if err != nil {
		panic(err)
	}

	client.authentication = authentication
	return client
}
