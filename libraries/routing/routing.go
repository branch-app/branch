package routing

import (
	"net/http"
	"regexp"
	"time"

	"github.com/branch-app/branch-mono-go/libraries/log"

	"gopkg.in/gin-gonic/gin.v1"
)

// MethodMetadata contains the structure of the metadata each method must provide when
// registering.
type MethodMetadata struct {
	Method          func(c *Context, v int64) *log.E
	DeprecationDate *int64
	Versions        []int64
}

// RegisteredMethods contains a map of every registered method.
var RegisteredMethods = make(map[string]MethodMetadata)
var errorMapping = map[string]int{
	"incompatible_accept_header": http.StatusNotAcceptable,
}

// RegisterMethod takes in information about the method and registers.
func RegisterMethod(methodName string, method func(c *Context, v int64) *log.E, deprecationDate string, versions []string) {
	methodMeta := MethodMetadata{
		Method:          method,
		Versions:        make([]int64, len(versions)),
		DeprecationDate: nil,
	}

	// Parse Versions
	for i, v := range versions {
		t, err := time.Parse("2006-01-02", v)
		if err != nil {
			panic(err)
		}

		methodMeta.Versions[i] = t.Unix()
	}

	// Parse Deprecation date
	if deprecationDate != "" {
		t, err := time.Parse("2006-01-02", deprecationDate)
		if err != nil {
			panic(err)
		}

		unix := t.Unix()
		methodMeta.DeprecationDate = &unix
	}

	RegisteredMethods[methodName] = methodMeta
}

func RouteRequest(c *gin.Context, app interface{}) {
	// Get method metadata
	method := c.Param("method")
	methodMetadata, ok := RegisteredMethods[method]
	if !ok {
		c.JSON(http.StatusInternalServerError, log.Info("method_not_found", log.M{"method": method}))
		return
	}

	// Verify version
	version, err := verifyMethodVersion(methodMetadata, c.Param("version"))
	if err != nil {
		c.JSON(http.StatusInternalServerError, err)
		return
	}

	// Create context
	context := &Context{
		App:        app,
		GinContext: c,
	}

	// Execute method and check for errors
	err = methodMetadata.Method(context, *version)
	if err != nil {
		status := http.StatusInternalServerError
		for c, s := range errorMapping {
			if c == err.Code {
				status = s
				break
			}
		}

		c.JSON(status, &err)
	}
}

func verifyMethodVersion(methodMetadata MethodMetadata, version string) (*int64, *log.E) {
	// Parse Versions
	unixVersion, err := getVersionDate(version)
	if err != nil {
		return nil, err
	}
	if unixVersion == nil {
		return nil, log.Info("preview_not_available", nil)
	}

	// Check if version is deprecated
	safeVersion := *unixVersion
	if methodMetadata.DeprecationDate != nil &&
		safeVersion > *methodMetadata.DeprecationDate {
		return nil, log.Info("deprecated_version", log.M{"version": version})
	}

	// Check if the version is valid
	validVersion := int64(-1)
	for _, v := range methodMetadata.Versions {
		if v <= safeVersion {
			validVersion = v
		}
	}

	// Check if the version is valid
	if validVersion == -1 {
		return nil, log.Info("version_not_found", log.M{"version": version})
	}

	return &validVersion, nil
}

func getVersionDate(str string) (*int64, *log.E) {
	switch str {
	case "latest":
		u := time.Now().UTC().Unix()
		return &u, nil
	case "preview":
		return nil, nil
	default:
		r := regexp.MustCompile(`[0-9]{4}-[0-9]{2}-[0-9]{2}`)
		if !r.MatchString(str) {
			return nil, log.Info("invalid_version", log.M{"version": str})
		}

		t, err := time.Parse("2006-01-02", str)
		if err != nil {
			return nil, log.Info("invalid_version", log.M{"version": str}, err)
		}

		u := t.Unix()
		return &u, nil
	}
}
