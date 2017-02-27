package clients

import (
	"strconv"
	"strings"
	"time"

	"net/http"

	"fmt"

	"github.com/branch-app/log-go"
	"github.com/branch-app/service-xboxlive/helpers"
	"github.com/branch-app/service-xboxlive/models"
	"github.com/branch-app/service-xboxlive/models/xboxlive"
	"github.com/branch-app/shared-go/clients"
)

type XboxLiveClient struct {
	httpClient    *clients.HTTPClient
	serviceClient *clients.ServiceClient
	xblStore      *helpers.XboxLiveStore
	mongoClient   *clients.MongoDBClient

	authentication    *models.XboxLiveAuthentication
	forceTokenRefresh bool
}

const (
	profileSettingsURL = "https://profile.xboxlive.com/users/%s(%s)/profile/settings?settings=gamertag"
	profileURL         = "https://profile.xboxlive.com/users/xuid(%s)/profile/settings?settings=GameDisplayPicRaw,Gamerscore,Gamertag,AccountTier,XboxOneRep,PreferredColor,RealName,Bio,TenureLevel,Watermarks,Location,ShowUserAsAvatar"

	authorizationHeaderFormat = "XBL3.0 x=%s;%s"
)

func (client *XboxLiveClient) GetProfileIdentity(identityCall *models.IdentityCall) *models.XboxLiveIdentity {
	// Check mem cache
	var identity *models.XboxLiveIdentity
	if identityCall.Type == "xuid" {
		identity = client.xblStore.GetByXUID(identityCall.Identity)
	} else if identityCall.Type == "gamertag" {
		identity = client.xblStore.GetByGT(identityCall.Identity)
	}

	// We still have a fresh identity, return it
	if identity != nil && identity.Fresh() {
		return identity
	}

	// Retrieve info
	url := fmt.Sprintf(profileSettingsURL, identityCall.Type, identityCall.Identity)
	var response xboxlive.ProfileUsers
	_, _, err := client.ExecuteRequest("GET", url, true, 2, nil, &response)
	if err != nil {
		panic(err)
	}

	// Pull data about profile
	settings := response.Users[0].Settings
	gamertag := settings[0].Value
	xuid := response.Users[0].XUID
	identity = models.NewXboxLiveIdentity(gamertag, xuid, time.Now().UTC())

	// Add to mem cache and return
	client.xblStore.Set(identity)
	return identity
}

func (client *XboxLiveClient) GetProfileSettings(identity *models.XboxLiveIdentity) *xboxlive.ProfileUsers {
	// Retrieve info
	url := fmt.Sprintf(profileURL, identity.XUID)
	var response xboxlive.ProfileUsers
	_, _, err := client.ExecuteRequest("GET", url, true, 3, nil, &response)
	if err != nil {
		panic(err)
	}

	return &response
}

func (client *XboxLiveClient) GetAuthentication() *models.XboxLiveAuthentication {
	if (client.authentication != nil && client.authentication.ExpiresAt.After(time.Now().UTC())) && !client.forceTokenRefresh {
		return client.authentication
	}

	var accountResp models.XboxLiveAuthResponse
	_, bErr, err := client.serviceClient.Get("service-auth", "/xbox-live", &accountResp)
	if err != nil {
		panic(err)
		if bErr != nil {
			panic(bErr)
		}
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
	_, bErr, err = client.ExecuteRequest("POST", "https://user.auth.xboxlive.com/user/authenticate", false, 0, xblAuthenticationRequest, &xblAuthenticationResponse)
	if err != nil {
		if bErr != nil {
			panic(bErr)
		}
		panic(err)
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
	_, bErr, err = client.ExecuteRequest("POST", "https://xsts.auth.xboxlive.com/xsts/authorize", false, 0, xblAuthorizationRequest, &xblAuthorizationResponse)
	if err != nil {
		if bErr != nil {
			panic(bErr)
		}
		panic(err)
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
	return client.authentication
}

func (client *XboxLiveClient) ExecuteRequest(method, endpoint string, auth bool, contractVersion int, body, respBody interface{}) (*http.Response, *branchlog.BranchError, error) {
	var authentication *models.XboxLiveAuthentication
	headers := map[string]string{
		"x-xbl-contract-version": strconv.Itoa(contractVersion),
	}

	if auth {
		authentication = client.GetAuthentication()
		headers["Authorization"] = fmt.Sprintf(authorizationHeaderFormat, authentication.UserHash, authentication.Token)
	}

	resp, err := client.httpClient.ExecuteRequest(method, endpoint, &headers, nil, body)
	if err != nil {
		if resp != nil && strings.Contains(resp.Header.Get("Content-Type"), clients.ContentTypeJSON) {
			var branchError branchlog.BranchError
			if err := clients.UnmarshalJSON(resp.Body, &branchError); err != nil {
				return nil, nil, err
			}

			return resp, &branchError, err
		}
		return nil, nil, err
	}

	err = clients.UnmarshalJSON(resp.Body, &respBody)
	return resp, nil, err
}

func NewXboxLiveClient(mongoConfig *models.MongoDBConfig) *XboxLiveClient {
	return &XboxLiveClient{
		httpClient:    clients.NewHTTPClient(),
		serviceClient: clients.NewServiceClient(),
		xblStore:      helpers.NewXboxLiveStore(),
		mongoClient:   clients.NewMongoDBClient(mongoConfig.ConnectionString, mongoConfig.DatabaseName),
	}
}
