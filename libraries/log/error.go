package log

import (
	"encoding/json"
	"reflect"

	"github.com/branch-app/branch-mono-go/helpers"

	"github.com/imdario/mergo"
)

// E implements the official Cuvva Error structure
type E struct {
	Code    string `json:"code"`
	Meta    M      `json:"meta,omitempty"`
	Reasons []E    `json:"reasons,omitempty"`
}

// M is an alias type for map[string]interface{}
type M map[string]interface{}

// Error returns the code of the Cuvva Error.
func (e E) Error() string {
	return e.Code
}

func newCuvvaError(code string, meta M, reasons []interface{}) *E {
	err := &E{
		Code:    code,
		Meta:    meta,
		Reasons: make([]E, len(reasons)),
	}

	for i, r := range reasons {
		err.Reasons[i] = Coerce(r)
	}

	return err
}

// Coerce attempts to coerce a Cuvva Error out of any object.
// - `E` types are just returned as-is
// - strings are json unmarshalled into a map[string]interface{} and recursive
// - bytes are parsed into a string and recursive
// - other primitives are parsed into
func Coerce(v interface{}) E {
	if v != nil {
		// If the interface is a pointer, make it not so
		rv := reflect.ValueOf(v)
		rt := reflect.TypeOf(v)

		if rv.Kind() == reflect.Ptr {
			return Coerce(reflect.Indirect(rv).Interface())
		}

		// If the interface is an empty array, todo
		if (rv.Kind() == reflect.Slice || rv.Kind() == reflect.Array) && rv.Len() == 0 {
			return E{
				Code: "unknown",
				Meta: M{
					"data": "empty_array",
				},
			}
		}

		// Check if interface is a E
		if err, ok := v.(E); ok {
			return err
		}

		// Check if interface is a go error
		if err, ok := reflect.Zero(rt).Interface().(error); ok {
			return E{Code: helpers.StrToSnakeTrimmed(err.Error(), 20)}
		}

		// Check if interface is a string
		if str, ok := v.(string); ok {
			return Coerce([]byte(str))
		}

		// Check if interface is a byte array/slice
		if bytes, ok := v.([]byte); ok {
			var d M
			if json.Unmarshal(bytes, &d) == nil {
				cErr := E{}

				if code, ok := d["code"].(string); ok {
					cErr.Code = helpers.StrToSnakeTrimmed(code, 20)
				} else {
					cErr.Code = "unknown"
				}

				if reasons, ok := d["reasons"].([]E); ok {
					cErr.Reasons = reasons
				}

				if meta, ok := d["meta"].(map[string]interface{}); ok {
					cErr.Meta = M(meta)
				} else {
					cErr.Meta = d
				}

				// Check if we need to remove the `code` from meta.
				if _, ok := cErr.Meta["code"]; ok {
					delete(cErr.Meta, "code")
				}

				return cErr
			}

			// The body isn't a valid json object
			return E{
				Code: "unknown",
				Meta: M{"data": string(bytes)},
			}
		}
	}

	// TODO: Catch all - if this happens, we can add a rule for that type
	return E{
		Code: "unable_to_coerce_error",
	}
}

// CoerceWithMeta runs Coerce on v, and then merges the metadata into the error response.
func CoerceWithMeta(v interface{}, meta M) E {
	err := Coerce(v)
	mergo.Merge(&err.Meta, meta)

	return err
}
