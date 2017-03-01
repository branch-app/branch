package clients

import (
	"errors"
	"strconv"
	"strings"
	"time"

	"net/http"

	"fmt"

	"github.com/branch-app/service-xboxlive/helpers"
	"github.com/branch-app/service-xboxlive/models"
	"github.com/branch-app/service-xboxlive/models/xboxlive"
	sharedClients "github.com/branch-app/shared-go/clients"
)

type XboxLiveClient struct {
	httpClient    *sharedClients.HTTPClient
	serviceClient *sharedClients.ServiceClient
	xblStore      *helpers.XboxLiveStore
	mongoClient   *sharedClients.MongoDBClient

	authentication    *models.XboxLiveAuthentication
	forceTokenRefresh bool
}

const (
	authorizationHeaderFormat = "XBL3.0 x=%s;%s"
)

func (client *XboxLiveClient) GetAuthentication() (*models.XboxLiveAuthentication, error) {
	if (client.authentication != nil && client.authentication.ExpiresAt.After(time.Now().UTC())) && !client.forceTokenRefresh {
		return client.authentication, nil
	}

	var accountResp models.XboxLiveAuthResponse
	_, err := client.serviceClient.Get("service-auth", "/xbox-live", &accountResp)
	if err != nil {
		return nil, err
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
	_, err = client.ExecuteRequest("POST", "https://user.auth.xboxlive.com/user/authenticate", false, 0, xblAuthenticationRequest, &xblAuthenticationResponse)
	if err != nil {
		return nil, err
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
	_, err = client.ExecuteRequest("POST", "https://xsts.auth.xboxlive.com/xsts/authorize", false, 0, xblAuthorizationRequest, &xblAuthorizationResponse)
	if err != nil {
		return nil, err
	}

	gamertag := xblAuthorizationResponse.DisplayClaims["xui"][0]["gtg"]
	userHash := xblAuthorizationResponse.DisplayClaims["xui"][0]["uhs"]
	xuid := xblAuthorizationResponse.DisplayClaims["xui"][0]["xid"]
	authentication := &models.XboxLiveAuthentication{
		ExpiresAt: time.Now().UTC().Add(55 * time.Minute),
		Identity:  models.NewXboxLiveIdentity(gamertag, xuid, time.Now().UTC()),
		Token:     xblAuthorizationResponse.Token,
		UserHash:  userHash,
	}

	client.forceTokenRefresh = false
	client.authentication = authentication
	fmt.Println(client.authentication)
	return client.authentication, nil
}

func (client *XboxLiveClient) ExecuteRequest(method, endpoint string, auth bool, contractVersion int, body, respBody interface{}) (*http.Response, error) {
	headers := map[string]string{
		"x-xbl-contract-version": strconv.Itoa(contractVersion),
	}

	if auth {
		authentication, err := client.GetAuthentication()
		if err != nil {
			return nil, err
		}
		headers["Authorization"] = fmt.Sprintf(authorizationHeaderFormat, authentication.UserHash, authentication.Token)
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
	case "identity_doesnt_exist":
		return http.StatusNotFound

	case "unknown_error":
	case "unknown_xbl_error_code":
	default:
		return http.StatusInternalServerError
	}

	return http.StatusInternalServerError
}

func (client *XboxLiveClient) handleError(response interface{}, err error) error {
	var resp *xboxlive.Response
	if response == nil {
		return err
	}
	resp, ok := response.(*xboxlive.Response)
	if !ok {
		return err
	}

	switch resp.Code {
	case xboxlive.ResponseUserDoesntExist:
		return errors.New("identity_doesnt_exist")

	default:
		return err
	}
}

func NewXboxLiveClient(mongoConfig *models.MongoDBConfig) *XboxLiveClient {
	return &XboxLiveClient{
		httpClient:    sharedClients.NewHTTPClient(),
		serviceClient: sharedClients.NewServiceClient(),
		xblStore:      helpers.NewXboxLiveStore(),
		mongoClient:   sharedClients.NewMongoDBClient(mongoConfig.ConnectionString, mongoConfig.DatabaseName),
	}
}
