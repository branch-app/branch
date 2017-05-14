package configloader

import (
	"encoding/json"
	"io/ioutil"
	"os"
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestUnmarshal(t *testing.T) {
	configFile := "./config.test.json"
	defer func() {
		// Remove fake config file
		if _, err := os.Stat(configFile); err == nil {
			os.Remove(configFile)
		}
	}()

	config := struct {
		Testing bool `json:"testing"`
	}{
		Testing: true,
	}

	data, err := json.Marshal(config)
	assert.Nil(t, err)

	err = ioutil.WriteFile(configFile, data, os.ModePerm)
	assert.Nil(t, err)

	err = Unmarshal("test", &config)
	assert.Nil(t, err)
	assert.True(t, config.Testing)
}
