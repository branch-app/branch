package log

import (
	"bytes"
	"log"
	"testing"
	"time"

	"github.com/stretchr/testify/assert"
)

func TestLevels(t *testing.T) {
	if LevelDebug >= LevelInfo {
		t.Error("invalid debug/info levels")
	}

	if LevelInfo >= LevelWarn {
		t.Error("invalid info/warn levels")
	}

	if LevelWarn >= LevelError {
		t.Error("invalid warn/error levels")
	}
}

func captureOutput(f func()) (string, string) {
	var bufOut bytes.Buffer
	var bufErr bytes.Buffer
	oldOut := logOut
	oldErr := logErr
	logOut = log.New(&bufOut, "", 0)
	logErr = log.New(&bufErr, "", 0)
	f()
	time.Sleep(5 * time.Millisecond)
	logOut = oldOut
	logErr = oldErr
	return bufOut.String(), bufErr.String()
}

func TestDebug(t *testing.T) {
	out, err := captureOutput(func() {
		Debug("blah", nil)
	})

	assert.Equal(t, "debug:{\"code\":\"blah\"}\n", out)
	assert.Zero(t, err)
}

func TestInfo(t *testing.T) {
	out, err := captureOutput(func() {
		Info("blah", nil)
	})

	assert.Equal(t, "info:{\"code\":\"blah\"}\n", out)
	assert.Zero(t, err)
}

func TestWarn(t *testing.T) {
	out, err := captureOutput(func() {
		Warn("blah", nil)
	})

	assert.Equal(t, "warn:{\"code\":\"blah\"}\n", err)
	assert.Zero(t, out)
}

func TestError(t *testing.T) {
	out, err := captureOutput(func() {
		Error("blah", nil)
	})

	assert.Equal(t, "error:{\"code\":\"blah\"}\n", err)
	assert.Zero(t, out)
}

func TestMeta(t *testing.T) {
	out, err := captureOutput(func() {
		Info("blah", M{"foo": "bar"})
	})

	assert.Equal(t, "info:{\"code\":\"blah\",\"meta\":{\"foo\":\"bar\"}}\n", out)
	assert.Zero(t, err)
}

func TestReasons(t *testing.T) {
	out, err := captureOutput(func() {
		Info("blah", M{"foo": "bar"}, "Example Data")
	})

	assert.Equal(t, "info:{\"code\":\"blah\",\"meta\":{\"foo\":\"bar\"},\"reasons\":[{\"code\":\"unknown\",\"meta\":{\"data\":\"Example Data\"}}]}\n", out)
	assert.Zero(t, err)
}
