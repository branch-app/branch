package response

// StatusCode is an enum representing Halo Waypoint response successfulness.
type StatusCode int

const (
	StatusCodeSuccess         StatusCode = 1
	StatusCodeContentNotFound StatusCode = 3
	StatusCodeNoData          StatusCode = 4
)
