package log

import (
	"crypto/rand"
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestErrorInterface(t *testing.T) {
	var err error = E{"test", nil, nil}

	assert.Equal(t, "test", err.Error())
}

func TestCoercedReasons(t *testing.T) {
	reasons := make([]interface{}, 2)
	reasons[0] = E{Code: "reason_error"}
	reasons[1] = "testing"

	err := newBranchError("testing", nil, reasons)
	assert.Equal(t, "testing", err.Code)
	assert.Equal(t, 2, len(err.Reasons))
	assert.Equal(t, "reason_error", err.Reasons[0].Code)
	assert.Equal(t, "unknown", err.Reasons[1].Code)
	assert.Equal(t, "testing", err.Reasons[1].Meta["data"])
}

func TestCoerceBranchError(t *testing.T) {
	validErr := Coerce(E{
		Code: "test_error",
		Meta: M{"meta": true},
	})
	validPointerErr := Coerce(&E{"test_error", nil, nil})

	assert.Equal(t, "test_error", validErr.Code)
	assert.True(t, validErr.Meta["meta"].(bool))
	assert.Equal(t, "test_error", validPointerErr.Code)
}

func TestCoerceString(t *testing.T) {
	jsonStrErr := Coerce(`{"code": "test_error", "meta": {"test": true}}`)
	strErr := Coerce("testing test")

	assert.Equal(t, "test_error", jsonStrErr.Code)
	assert.True(t, jsonStrErr.Meta["test"].(bool))
	assert.Equal(t, "unknown", strErr.Code)
	assert.Equal(t, "testing test", strErr.Meta["data"].(string))
}

func generateRandomBytes(n int) []byte {
	b := make([]byte, n)
	rand.Read(b)
	return b
}
