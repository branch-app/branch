package auth

import (
	"net/http"
	"testing"

	"fmt"

	"time"

	"github.com/branch-app/branch-mono-go/domain/auth"
	"github.com/stretchr/testify/assert"
	"gopkg.in/h2non/gock.v1"
)

func TestNewClient(t *testing.T) {
	key := "test_key"
	baseURL := "http://svc-auth.branch/"
	client := NewClient(baseURL, key)
	options := client.jsonClient.Options()

	assert.Equal(t, fmt.Sprintf("bearer %s", key), options.Headers["Authorization"])
	assert.Equal(t, baseURL, client.jsonClient.BaseURL())
}

func TestGetHalo4Token(t *testing.T) {
	defer gock.Off()

	client := NewClient("http://svc-auth.branch/", "test_key")
	time, _ := time.Parse("May 5, 2017 at 9:42pm (UTC)", "2017-05-13T09:42:42Z")
	json := auth.Halo4Token{
		SpartanToken:   "test_spartan_token",
		AnalyticsToken: "test_analytics_token",
		ExpiresAt:      time,
		Gamertag:       "Program",
	}

	gock.New("http://svc-auth.branch/").
		Post("/1/2017-05-13/get_halo4_token").
		Reply(http.StatusOK).
		JSON(json)
	gock.InterceptClient(client.jsonClient.HTTPClient)

	token, err := client.GetHalo4Token()
	assert.Nil(t, err)
	assert.Equal(t, json.SpartanToken, token.SpartanToken)
	assert.Equal(t, json.AnalyticsToken, token.AnalyticsToken)
	assert.Equal(t, json.ExpiresAt, token.ExpiresAt)
	assert.Equal(t, json.Gamertag, token.Gamertag)
	assert.True(t, gock.IsDone())
}

func TestGetXboxLiveToken(t *testing.T) {
	defer gock.Off()

	client := NewClient("http://svc-auth.branch/", "test_key")
	time, _ := time.Parse("May 5, 2017 at 9:42pm (UTC)", "2017-05-13T09:42:42Z")
	json := auth.XboxLiveToken{
		ExpiresAt: time,
		Gamertag:  "Program",
		Token:     "test_token",
		UserHash:  "test_hash",
		XUID:      "test_xuid",
	}

	gock.New("http://svc-auth.branch/").
		Post("/1/2017-05-13/get_xboxlive_token").
		Reply(http.StatusOK).
		JSON(json)
	gock.InterceptClient(client.jsonClient.HTTPClient)

	token, err := client.GetXboxLiveToken()
	assert.Nil(t, err)
	assert.Equal(t, json.ExpiresAt, token.ExpiresAt)
	assert.Equal(t, json.Gamertag, token.Gamertag)
	assert.Equal(t, json.Token, token.Token)
	assert.Equal(t, json.UserHash, token.UserHash)
	assert.Equal(t, json.XUID, token.XUID)
	assert.True(t, gock.IsDone())
}
