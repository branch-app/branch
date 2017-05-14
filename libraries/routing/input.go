package routing

import (
	"bytes"
	"encoding/json"
	"mime"

	"github.com/branch-app/branch-mono-go/libraries/jsonschemaclient"
	"github.com/branch-app/branch-mono-go/libraries/log"
)

var jsonSchemaClient *jsonschemaclient.Client

func init() {
	jsonSchemaClient = jsonschemaclient.NewClient()
}

// ParseInput takes in the request context, verifies it conforms to the json schema, and
// returns it into the data interface provided.
func ParseInput(c *Context, data interface{}, schemaName string) *log.E {
	req := c.GinContext.Request
	contentType := req.Header.Get("Content-Type")

	if contentType == "" {
		return nil
	}

	mediaType, _, err := mime.ParseMediaType(contentType)
	if err != nil {
		return log.Info("media_type_parse_failed", nil, err)
	}

	switch mediaType {
	default:
		return log.Info("unsupported_media_type", nil)

	case "application/json":
		buf := new(bytes.Buffer)
		buf.ReadFrom(req.Body)
		body := buf.Bytes()

		// Check JSON Schema
		err := jsonSchemaClient.Validate(schemaName, body)
		if err != nil {
			return err
		}

		// Unmarshal JSON
		if err := json.Unmarshal(body, &data); err != nil {
			return log.Info("json_marshal_failed", nil, err)
		}

		return nil
	}
}
