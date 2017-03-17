package clients

import (
	"errors"
	"strconv"
	"strings"
	"time"

	"net/http"

	"fmt"

	log "github.com/branch-app/log-go"
	"github.com/branch-app/service-xboxlive/helpers"
	"github.com/branch-app/service-xboxlive/models"
	"github.com/branch-app/service-xboxlive/models/xboxlive"
	sharedClients "github.com/branch-app/shared-go/clients"
	sharedModels "github.com/branch-app/shared-go/models"
	"github.com/branch-app/shared-go/types"
	"gopkg.in/robfig/cron.v2"
)

type XboxLiveClient struct {
	httpClient    *sharedClients.HTTPClient
	serviceClient *sharedClients.ServiceClient
	xblStore      *helpers.XboxLiveStore
	mongoClient   *sharedClients.MongoDBClient

	authentication *models.XboxLiveAuthentication
	cron           *cron.Cron
}

const (
	ErrorUnableToAuthenticateWithXbl = "unable_to_authenticate_with_xboxlive"
	ErrorIdentityDoesntExist         = "identity_doesnt_exist"
	ErrorInternalAuthDown            = "internal_auth_down"
	ErrorUnknown                     = "unknown_error"

	authorizationHeaderFormat = "XBL3.0 x=%s;%s"
)

func (client *XboxLiveClient) GetAuthentication() (*models.XboxLiveAuthentication, error) {
	if client.authentication != nil && client.authentication.ExpiresAt.After(time.Now().UTC()) {
		return client.authentication, nil
	}

	auth, err := client.UpdateAuthentication()
	if err != nil {
		return nil, log.Error(ErrorUnableToAuthenticateWithXbl, &log.M{"error": err}, nil).ToError()
	}
	return auth, nil
}

func (client *XboxLiveClient) UpdateAuthentication() (*models.XboxLiveAuthentication, error) {
	var accountResp models.XboxLiveAuthResponse
	_, err := client.serviceClient.Get("service-auth", "/xbox-live", &accountResp)
	if err != nil {
		return nil, log.Error(ErrorUnableToAuthenticateWithXbl, &log.M{"error": err}, nil).ToError()
	}

	xblAuthenticationRequest := &models.XboxLiveAuthenticationRequest{
		Properties: &models.XboxLiveAuthenticationPropertiesRequest{
			AuthMethod: "RPS",
			RpsTicket:  fmt.Sprintf("t=%s", accountResp.AccessToken),
			SiteName:   "user.auth.xboxlive.com",
		},
		RelyingParty: "http://auth.xboxlive.com",
		TokenType:    "JWT",
	}
	var xblAuthenticationResponse models.XboxLiveAuthenticationResponse
	_, err = client.ExecuteRequest("POST", "https://user.auth.xboxlive.com/user/authenticate", nil, 0, xblAuthenticationRequest, &xblAuthenticationResponse)
	if err != nil {
		return nil, log.Error(ErrorUnableToAuthenticateWithXbl, &log.M{"error": err}, nil).ToError()
	}

	xblAuthorizationRequest := &models.XboxLiveAuthorizationRequest{
		Properties: &models.XboxLiveAuthorizationPropertiesRequest{
			SandboxID:  "RETAIL",
			UserTokens: []string{xblAuthenticationResponse.Token},
		},
		RelyingParty: "http://xboxlive.com",
		TokenType:    "JWT",
	}
	var xblAuthorizationResponse models.XboxLiveAuthorizationResponse
	_, err = client.ExecuteRequest("POST", "https://xsts.auth.xboxlive.com/xsts/authorize", nil, 0, xblAuthorizationRequest, &xblAuthorizationResponse)
	if err != nil {
		return nil, log.Error(ErrorUnableToAuthenticateWithXbl, &log.M{"error": err}, nil).ToError()
	}

	gamertag := xblAuthorizationResponse.DisplayClaims["xui"][0]["gtg"]
	userHash := xblAuthorizationResponse.DisplayClaims["xui"][0]["uhs"]
	xuid := xblAuthorizationResponse.DisplayClaims["xui"][0]["xid"]
	authentication := &models.XboxLiveAuthentication{
		ExpiresAt: time.Now().UTC().Add(55 * time.Minute),
		Identity:  sharedModels.NewXboxLiveIdentity(gamertag, xuid, time.Now().UTC()),
		Token:     xblAuthorizationResponse.Token,
		UserHash:  userHash,
	}

	client.authentication = authentication
	fmt.Println(client.authentication)
	return client.authentication, nil
}

func (client *XboxLiveClient) ExecuteRequest(method, endpoint string, auth *models.XboxLiveAuthentication, contractVersion int, body, respBody interface{}) (*http.Response, error) {
	headers := map[string]string{
		"x-xbl-contract-version": strconv.Itoa(contractVersion),
	}

	if auth != nil {
		headers["Authorization"] = fmt.Sprintf(authorizationHeaderFormat, auth.UserHash, auth.Token)
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

func (client *XboxLiveClient) ErrorToHTTPStatus(err error) int {
	switch err.Error() {
	case ErrorIdentityDoesntExist:
		return http.StatusNotFound

	case ErrorUnknown:
	default:
		return http.StatusInternalServerError
	}

	return http.StatusInternalServerError
}

func (client *XboxLiveClient) handleError(response interface{}, err error) error {
	if response != nil {
		var resp *xboxlive.Response
		resp, ok := response.(*xboxlive.Response)
		if ok {
			switch resp.Code {
			case xboxlive.ResponseUserDoesntExist:
				return errors.New(ErrorIdentityDoesntExist)
			}
		}
	}

	if err.Error() == ErrorUnableToAuthenticateWithXbl {
		return errors.New(ErrorInternalAuthDown)
	}

	// unknown_error
	return errors.New(ErrorUnknown)
}

func NewXboxLiveClient(env types.Environment, config *models.Configuration) *XboxLiveClient {
	client := &XboxLiveClient{
		httpClient:    sharedClients.NewHTTPClient(),
		serviceClient: sharedClients.NewServiceClient(env),
		xblStore:      helpers.NewXboxLiveStore(),
		mongoClient:   sharedClients.NewMongoDBClient(config.MongoConnectionString, config.MongoDatabaseName),
		cron:          cron.New(),
	}

	// Setup cron jobs
	client.cron.AddFunc("@every 45m", func() { client.UpdateAuthentication() })
	client.cron.Start()

	// Update authentication in the background
	go client.UpdateAuthentication()

	return client
}
