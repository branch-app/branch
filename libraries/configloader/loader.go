package configloader

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
)

// Unmarshal loads a JSON config file into an interface.
func Unmarshal(boot string, v interface{}) error {
	file, err := ioutil.ReadFile(fmt.Sprintf("./config.%s.json", boot))
	if err != nil {
		return err
	}

	return json.Unmarshal(file, &v)
}
