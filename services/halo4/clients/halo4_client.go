package clients

import (
	"errors"
	"net/http"
	"strings"
	"time"

	log "github.com/branch-app/log-go"
	"github.com/branch-app/service-halo4/models"
	"github.com/branch-app/service-halo4/models/halo4"
	sharedClients "github.com/branch-app/shared-go/clients"
	"github.com/branch-app/shared-go/types"
	"gopkg.in/robfig/cron.v2"
)

type Halo4Client struct {
	httpClient    *sharedClients.HTTPClient
	serviceClient *sharedClients.ServiceClient
	mongoClient   *sharedClients.MongoDBClient

	authentication *models.WaypointAuthentication
	cron           *cron.Cron
}

const (
	ErrorUnableToAuthenticateWithWaypoint = "unable_to_authenticate_with_waypoint"
	ErrorInternalAuthDown                 = "internal_auth_down"
	ErrorContentNotFound                  = "waypoint_content_not_found"
	ErrorNoData                           = "waypoint_no_data"
	ErrorUnknown                          = "unknown_error"

	AuthorizationHeaderName = "X-343-Authorization-Spartan"
)

func (client *Halo4Client) GetAuthentication() (*models.WaypointAuthentication, error) {
	// If we on first run, wait 9 seconds
	if client.authentication == nil {
		time.Sleep(time.Second * 9)
	}

	// Pull in authentication
	if client.authentication != nil && client.authentication.ExpiresAt.After(time.Now().UTC()) {
		return client.authentication, nil
	}

	// Something weird happened, try getting auth again
	return client.UpdateAuthentication()
}

func (client *Halo4Client) UpdateAuthentication() (*models.WaypointAuthentication, error) {
	var auth *models.WaypointAuthentication
	_, err := client.serviceClient.Get("service-auth", "/halo-4", &auth)
	if err != nil {
		return nil, log.Error(ErrorUnableToAuthenticateWithWaypoint, &log.M{"error": err}, nil).ToError()
	}

	auth.ExpiresAt = time.Now().UTC().Add(50 * time.Minute)
	client.authentication = auth
	return client.authentication, nil
}

func (client *Halo4Client) ExecuteRequest(method, endpoint string, auth *models.WaypointAuthentication, body, respBody interface{}) (*http.Response, error) {
	headers := map[string]string{
		"Accept": "application/json",
	}

	if auth != nil {
		headers[AuthorizationHeaderName] = auth.SpartanToken
	}

	resp, err := client.httpClient.ExecuteRequest(method, endpoint, &headers, nil, body)
	if err != nil {
		if resp != nil && strings.Contains(resp.Header.Get("Content-Type"), sharedClients.ContentTypeJSON) {
			if err := sharedClients.UnmarshalJSON(resp.Body, &respBody); err != nil {
				return nil, err
			}

			return resp, err
		}
		return nil, err
	}

	err = sharedClients.UnmarshalJSON(resp.Body, &respBody)
	return resp, err
}

func (client *Halo4Client) ErrorToHTTPStatus(err error) int {
	switch err.Error() {
	case ErrorContentNotFound:
	case ErrorNoData:
		return http.StatusNotFound

	case ErrorUnknown:
	default:
		return http.StatusInternalServerError
	}

	return http.StatusInternalServerError
}

func (client *Halo4Client) handleError(response *halo4.Response, err error) error {
	if response != nil {
		switch response.StatusCode {
		case halo4.StatusCodeContentNotFound:
			return errors.New(ErrorContentNotFound)
		case halo4.StatusCodeNoData:
			return errors.New(ErrorNoData)
		}
	}

	if err != nil && err.Error() == ErrorUnableToAuthenticateWithWaypoint {
		return errors.New(ErrorInternalAuthDown)
	}

	// unknown_error
	return errors.New(ErrorUnknown)
}

func NewHalo4Client(env types.Environment, config *models.Configuration) *Halo4Client {
	client := &Halo4Client{
		httpClient:    sharedClients.NewHTTPClient(),
		serviceClient: sharedClients.NewServiceClient(env),
		mongoClient:   sharedClients.NewMongoDBClient(config.Mongo.Host, config.Mongo.Database, config.Mongo.Username, config.Mongo.Password),
		cron:          cron.New(),
	}

	// Setup cron jobs
	client.cron.AddFunc("@every 45m", func() { client.UpdateAuthentication() })
	client.cron.Start()

	// Update authentication in the background
	go client.UpdateAuthentication()

	return client
}
