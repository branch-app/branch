package xboxlive

// IdentityLookup defines the structure of an Identity Request
type IdentityLookup struct {
	Value string `json:"value"`
	Type  string `json:"type"`
}
