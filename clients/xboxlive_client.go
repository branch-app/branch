package clients

import (
	"errors"
	"strconv"
	"strings"
	"time"

	"net/http"

	"fmt"

	"github.com/branch-app/log-go"
	"github.com/branch-app/service-xboxlive/helpers"
	"github.com/branch-app/service-xboxlive/models"
	"github.com/branch-app/service-xboxlive/models/xboxlive"
	sharedClients "github.com/branch-app/shared-go/clients"
	sharedHelpers "github.com/branch-app/shared-go/helpers"
	"gopkg.in/mgo.v2/bson"
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
	profileSettingsURL = "https://profile.xboxlive.com/users/%s(%s)/profile/settings?settings=gamertag"
	profileURL         = "https://profile.xboxlive.com/users/xuid(%s)/profile/settings?settings=GameDisplayPicRaw,Gamerscore,Gamertag,AccountTier,XboxOneRep,PreferredColor,RealName,Bio,TenureLevel,Watermarks,Location,ShowUserAsAvatar"

	authorizationHeaderFormat = "XBL3.0 x=%s;%s"
)

func (client *XboxLiveClient) GetProfileIdentity(identityCall *models.IdentityCall) (*models.XboxLiveIdentity, error) {
	// Check mem cache
	var identity *models.XboxLiveIdentity
	if identityCall.Type == "xuid" {
		identity = client.xblStore.GetByXUID(identityCall.Identity)
	} else if identityCall.Type == "gamertag" {
		identity = client.xblStore.GetByGT(identityCall.Identity)
	}

	// We still have a fresh identity, return it
	if identity != nil && identity.Fresh() {
		return identity, nil
	}

	// Retrieve info
	url := fmt.Sprintf(profileSettingsURL, identityCall.Type, identityCall.Identity)
	var response xboxlive.ProfileUsers
	_, err := client.ExecuteRequest("GET", url, true, 2, nil, &response)
	if err != nil {
		return nil, client.handleError(&response.Response, err)
	}

	// Pull data about profile
	settings := response.Users[0].Settings
	gamertag := settings[0].Value
	xuid := response.Users[0].XUID
	identity = models.NewXboxLiveIdentity(gamertag, xuid, time.Now().UTC())

	// Add to mem cache and return
	client.xblStore.Set(identity)
	return identity, nil
}

func (client *XboxLiveClient) GetProfileSettings(identity *models.XboxLiveIdentity) (*xboxlive.ProfileUsers, error) {
	url := fmt.Sprintf(profileURL, identity.XUID)
	urlHash := sharedHelpers.CreateSHA512Hash(url)

	// Check if we have a cached document
	cacheRecord, err := xboxlive.CacheRecordFindOne(client.mongoClient, bson.M{"doc_url_hash": urlHash})
	if err != nil {
		return nil, err
	}
	if cacheRecord != nil && cacheRecord.IsValid() {
		response, err := xboxlive.ProfileUsersFindOne(client.mongoClient, bson.M{"_id": cacheRecord.DocumentID})
		if err != nil {
			return nil, err
		}
		return response, nil
	}

	// Retrieve data from Xbox Live
	var profileUsers *xboxlive.ProfileUsers
	_, err = client.ExecuteRequest("GET", url, true, 3, nil, &profileUsers)
	if err != nil {
		return nil, client.handleError(&profileUsers.Response, err)
	}

	// Deal with caching
	if cacheRecord != nil {
		// Already have a cache record - so just update
		profileUsers.Id = cacheRecord.DocumentID
		profileUsers.Save(client.mongoClient)

		// Update Cache Record
		cacheRecord.Update(client.mongoClient)
	} else {
		// Need to add to database
		profileUsers.Save(client.mongoClient)
		cacheRecord = xboxlive.NewCacheRecord(identity.XUID, url, profileUsers.Id, 5*time.Minute)
		cacheRecord.Save(client.mongoClient)
	}

	return profileUsers, nil
}

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

func (client *XboxLiveClient) handleError(response *xboxlive.Response, err error) error {
	if response == nil {
		branchlog.Error("unknown_error", nil, &map[string]interface{}{
			"error": err,
		})
		return errors.New("unknown_error")
	}

	fmt.Println(response)
	switch response.Code {
	case xboxlive.ResponseUserDoesntExist:
		branchlog.Error("identity_doesnt_exist", nil, &map[string]interface{}{
			"error": err,
		})
		return errors.New("identity_doesnt_exist")

	default:
		branchlog.Error("unknown_xbl_error_code", nil, &map[string]interface{}{
			"error":    err,
			"response": response,
		})
		return errors.New("unknown_xbl_error_code")
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
