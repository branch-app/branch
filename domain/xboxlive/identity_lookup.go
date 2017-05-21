package xboxlive

// IdentityLookup defines the structure of an Identity Request
type IdentityLookup struct {
	Value string `json:"value" bson:"value"`
	Type  string `json:"type" bson:"type"`
}
