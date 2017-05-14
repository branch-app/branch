package crypto

import (
	"crypto/sha512"
	"encoding/base64"
)

func CreateSHA512Hash(str string) string {
	hasher := sha512.New()
	hasher.Write([]byte(str))
	return base64.URLEncoding.EncodeToString(hasher.Sum(nil))
}
