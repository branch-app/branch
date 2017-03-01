package branch

import "time"

// Response wraps all branch responses with relevant metadata about caching
type Response struct {
	CachedAt time.Time `json:"cachedAt"`
	// ApiUp    bool        `json:"api_up"`
	Data interface{} `json:"data"`
}

func NewResponse(cachedAt time.Time, data interface{}) *Response {
	return &Response{
		CachedAt: cachedAt,
		// ApiUp:    apiUp,
		Data: data,
	}
}
