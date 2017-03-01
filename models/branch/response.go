package branch

import (
	"time"
)

// Response wraps all branch responses with relevant metadata about caching
type Response struct {
	CacheDate time.Time   `json:"cache_date"`
	Data      interface{} `json:"data"`
}
