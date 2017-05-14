package jsonclient

import (
	"testing"

	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/stretchr/testify/assert"
	"gopkg.in/h2non/gock.v1"
)

func TestGetHTTPMethod(t *testing.T) {
	defer gock.Off()

	gock.New("http://coo.va/").
		Get("/test").
		Reply(http.StatusNoContent)

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	err := client.Do("GET", "test", nil, nil, nil)
	assert.Nil(t, err)
	assert.True(t, gock.IsDone())
}

func TestPutHTTPMethod(t *testing.T) {
	defer gock.Off()

	gock.New("http://coo.va/").
		Put("/test").
		Reply(http.StatusNoContent)

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	err := client.Do("PUT", "test", nil, nil, nil)
	assert.Nil(t, err)
	assert.True(t, gock.IsDone())
}

func TestPostHTTPMethod(t *testing.T) {
	defer gock.Off()

	gock.New("http://coo.va/").
		Post("/test").
		Reply(http.StatusNoContent)

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	err := client.Do("POST", "test", nil, nil, nil)
	assert.Nil(t, err)
	assert.True(t, gock.IsDone())
}

func TestDeleteHTTPMethod(t *testing.T) {
	defer gock.Off()

	gock.New("http://coo.va/").
		Delete("/test").
		Reply(http.StatusNoContent)

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	err := client.Do("DELETE", "test", nil, nil, nil)
	assert.Nil(t, err)
	assert.True(t, gock.IsDone())
}

func TestRequestQuery(t *testing.T) {
	defer gock.Off()

	paramKey := "testing"
	paramValue := "true"

	gock.New("http://coo.va/").
		Get("/test").
		MatchParam(paramKey, paramValue).
		Reply(http.StatusNoContent)

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	err := client.Do("GET", "test", nil, nil, &DoOptions{
		Query: M{paramKey: paramValue},
	})
	assert.Nil(t, err)
	assert.True(t, gock.IsDone())
}

func TestRequestHeaders(t *testing.T) {
	defer gock.Off()

	headerKey := "X-Testing-Value"
	headerValue := "LetsAllPlayNice!"

	gock.New("http://coo.va/").
		Get("/test").
		MatchHeader(headerKey, headerValue).
		Reply(http.StatusNoContent)

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	err := client.Do("GET", "test", nil, nil, &DoOptions{
		Headers: M{headerKey: headerValue},
	})
	assert.Nil(t, err)
	assert.True(t, gock.IsDone())
}

func TestRequestBody(t *testing.T) {
	defer gock.Off()

	testJSON := map[string]bool{"testing": true}

	gock.New("http://coo.va/").
		Post("/test").
		JSON(testJSON).
		Reply(http.StatusNoContent)

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	err := client.Do("POST", "test", testJSON, nil, nil)
	assert.Nil(t, err)
	assert.True(t, gock.IsDone())
}

func TestResponseBody(t *testing.T) {
	defer gock.Off()

	gock.New("http://coo.va/").
		Get("/test").
		MatchHeader("Accept", "application/json").
		Reply(http.StatusNoContent).
		JSON(map[string]bool{"testing": true})

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	var response map[string]bool
	err := client.Do("GET", "test", nil, &response, nil)
	assert.Nil(t, err)
	assert.True(t, response["testing"])
	assert.True(t, gock.IsDone())
}

func TestErrorUnmarshaling(t *testing.T) {
	defer gock.Off()

	responseError := &log.E{Code: "test_error"}

	gock.New("http://coo.va/").
		Get("/test").
		Reply(http.StatusBadRequest).
		JSON(responseError)

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	err := client.Do("GET", "test", nil, nil, nil)
	assert.NotNil(t, err)
	assert.Equal(t, responseError.Code, err.Code)
	assert.True(t, gock.IsDone())
}

func TestErrorCatching(t *testing.T) {
	defer gock.Off()

	gock.New("http://coo.va/").
		Get("/test").
		Reply(http.StatusInternalServerError)

	client := NewClient("http://coo.va/", nil)
	gock.InterceptClient(client.HTTPClient)

	err := client.Do("GET", "test", nil, nil, nil)
	assert.NotNil(t, err)
	assert.Equal(t, err.Code, "unknown")
	assert.True(t, gock.IsDone())
}
