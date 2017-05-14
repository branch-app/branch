package jsonschemaclient

import (
	"io/ioutil"
	"os"
	"path"
	"path/filepath"
	"strings"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/xeipuuv/gojsonschema"
)

// Client is the structure of the JSON Schema Client core struct.
type Client struct {
	schemaLocation string
	schemaLoaders  map[string]gojsonschema.JSONLoader
}

// NewClient creates a new JSON Schema Client. It reads all the schema files in the
// /schema/ directory inside the applications working directory, and stores them in memory
// for quick validation.
func NewClient() *Client {
	// Get application working directory
	wd, err := os.Getwd()
	if err != nil {
		panic(err)
	}

	// Generate schema path
	schemaDir := path.Join(wd, "/schema/")

	// Read all json schema files
	files, _ := ioutil.ReadDir(schemaDir)
	schemaLoaders := make(map[string]gojsonschema.JSONLoader)
	for _, f := range files {
		name := f.Name()
		if !strings.HasSuffix(name, ".json") {
			break
		}

		schemaFileName := path.Join(schemaDir, name)
		schemaName := strings.TrimSuffix(name, filepath.Ext(name))
		schemaLoader := gojsonschema.NewReferenceLoader("file://" + schemaFileName)
		schemaLoaders[schemaName] = schemaLoader
	}

	return &Client{
		schemaLocation: schemaDir,
		schemaLoaders:  schemaLoaders,
	}
}

// Validate takes in the name of the json schema (minus .json) and the json data and
// validates it against the schema.
func (client *Client) Validate(schemaName string, jsonData []byte) *log.E {
	schemaLoader, ok := client.schemaLoaders[schemaName]

	if !ok {
		return log.Warn("invalid_schema_name", nil)
	}

	documentLoader := gojsonschema.NewBytesLoader(jsonData)
	result, err := gojsonschema.Validate(schemaLoader, documentLoader)
	if err != nil {
		return log.Error("invalid_body", nil, err)
	}

	if result.Valid() {
		return nil
	}

	errs := coerceJSONSchemaErrors(result.Errors())
	return log.Warn("invalid_body", nil, errs...)
}

func coerceJSONSchemaErrors(jsonErrors []gojsonschema.ResultError) []interface{} {
	errors := make([]interface{}, len(jsonErrors))
	for i, e := range jsonErrors {
		errors[i] = &log.E{
			Code: "invalid_field",
			Meta: log.M{
				"field":   e.Field(),
				"message": e.Description(),
			},
		}
	}

	return errors
}
