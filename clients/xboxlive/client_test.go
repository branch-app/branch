package xboxlive

import (
	"net/http"
	"testing"

	"fmt"

	"github.com/branch-app/branch-mono-go/domain/xboxlive"
	"github.com/stretchr/testify/assert"
	"gopkg.in/h2non/gock.v1"
)

func TestNewClient(t *testing.T) {
	key := "test_key"
	baseURL := "http://svc-xboxlive.branch/"
	client := NewClient(baseURL, key)
	options := client.jsonClient.Options()

	assert.Equal(t, fmt.Sprintf("bearer %s", key), options.Headers["Authorization"])
	assert.Equal(t, baseURL, client.jsonClient.BaseURL())
}

func TestGetIdentity(t *testing.T) {
	defer gock.Off()

	client := NewClient("http://svc-xboxlive.branch/", "test_key")
	json := xboxlive.Identity{
		Gamertag: "Program",
		XUID:     "837493658656373935",
	}

	gock.New("http://svc-xboxlive.branch/").
		Post("/1/2017-05-14/get_identity").
		Reply(http.StatusOK).
		JSON(json)
	gock.InterceptClient(client.jsonClient.HTTPClient)

	identity, err := client.GetIdentity(xboxlive.IdentityLookup{Type: "gamertag", Value: "Program"})
	assert.Nil(t, err)
	assert.Equal(t, identity.Gamertag, identity.Gamertag)
	assert.Equal(t, identity.XUID, identity.XUID)
	assert.True(t, gock.IsDone())
}
