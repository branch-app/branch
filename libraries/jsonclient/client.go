package jsonclient

import (
	"bytes"
	"encoding/json"
	"io"
	"net/http"
	"net/url"
	"strings"
	"time"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/imdario/mergo"
)

// M is an alias type of map[string]string
type M map[string]string

// Client is the structure of the JSON Client core struct.
type Client struct {
	// HTTPClient is exported to be used with tests - do not modify it through the struct.
	HTTPClient *http.Client

	baseURL string
	options ClientOptions
}

// ClientOptions is the structure of the Options for the JSON Client.
type ClientOptions struct {
	// Headers is a map of default headers.
	Headers M

	// The timeout for the request.
	Timeout time.Duration
}

// DoOptions is the structure of the Request
type DoOptions struct {
	// Headers is a map of default headers.
	Headers M

	// Query is a map of URL Query params.
	Query M
}

// NewClient creates a new JSON client, based on the specified options. If the options
// are nil, then defaults will be applied.
// Defaults:
// - Headers: `Accept: application/json`
// - Timeout: 2 seconds
func NewClient(baseURL string, options *ClientOptions) *Client {
	if options == nil {
		options = &ClientOptions{
			Headers: M{"Accept": "application/json"},
			Timeout: 2 * time.Second,
		}
	}

	// Make sure baseURL ends with /
	if !strings.HasSuffix(baseURL, "/") {
		baseURL = baseURL + "/"
	}

	return &Client{
		baseURL: baseURL,
		HTTPClient: &http.Client{
			Timeout: options.Timeout,
		},
		options: *options,
	}
}

// Do makes an HTTP request based on the provided params.
// - method is the HTTP method verb of the request (GET/PUT/PATCH/POST/DELETE)
// - endpoint is the non '/' preffixed endpoint to request
// - body is an interface of the content to send - use nil if no body is being sent.
// - response is an interface the response of the request will be un-marshalled into -
//   use nil if you don't want to receive the response.
// - options is an instance of the 'DoOptions' struct, defining request specific options -
//   use nil if you don't want to set any options.
func (client *Client) Do(method, endpoint string, body, response interface{}, options *DoOptions) *log.E {
	// Ensure endpoint doesn't begin have a
	endpoint = strings.TrimPrefix(endpoint, "/")

	// Parse URL
	url, err := url.Parse(client.baseURL + endpoint)
	if err != nil {
		return log.Warn("invalid_endpoint", nil, err)
	}

	// Merge Options
	if options == nil {
		options = &DoOptions{}
	}

	mergo.Merge(&options.Headers, client.options.Headers)

	// Add Query strings
	if len(options.Query) > 0 {
		q := url.Query()
		for k, v := range options.Query {
			q.Add(k, v)
		}
		url.RawQuery = q.Encode()
	}

	// Create Request the request
	var req *http.Request
	if body != nil {
		mergo.Merge(&options.Headers, M{"Content-Type": "application/json"})
		data, err := json.Marshal(body)
		if err != nil {
			return log.Warn("json_marshal_failed", nil, err)
		}

		req, err = http.NewRequest(method, url.String(), bytes.NewBuffer(data))
	} else {
		req, err = http.NewRequest(method, url.String(), nil)
	}

	// Check if creating the request failed
	if err != nil {
		return log.Warn("request_creation_failed", nil, err)
	}

	// Add Headers
	for k, v := range options.Headers {
		req.Header.Add(k, v)
	}

	// Execute Request
	resp, err := client.HTTPClient.Do(req)
	if err != nil {
		return log.Warn("request_execution_failure", nil, err)
	}

	// Check response
	if resp.StatusCode >= http.StatusOK && resp.StatusCode < http.StatusBadRequest {
		if response != nil {
			if branchErr := unmarshalJSON(resp.Body, &response); branchErr != nil {
				return branchErr
			}
		}
		return nil
	}

	// Read response body
	responseBuffer := new(bytes.Buffer)
	responseBuffer.ReadFrom(resp.Body)

	// Coerce response
	cErr := log.CoerceWithMeta(responseBuffer.Bytes(), log.M{
		"httpStatus": resp.Status,
		"method":     req.Method,
		"url":        url.String(),
	})

	return &cErr
}

// Options returns the global set of options for every request made with this instance of
// json client.
func (client *Client) Options() ClientOptions {
	return client.options
}

// BaseURL returns the base url of this instance of json client.
func (client *Client) BaseURL() string {
	return client.baseURL
}

// unmarshalJSON reads the JSON from a body stream into an interface.
func unmarshalJSON(body io.ReadCloser, v interface{}) *log.E {
	defer body.Close()

	err := json.NewDecoder(body).Decode(&v)
	if err != nil {
		return log.Warn("json_unmarshal_failed", nil, err)
	}

	return nil
}
