package xboxlive

type ResponseCode int

type Response struct {
	Code             ResponseCode `json:"code"`
	Source           string               `json:"source"`
	Description      string               `json:"description"`
	TraceInformation string               `json:"tranceInformation"`
}
