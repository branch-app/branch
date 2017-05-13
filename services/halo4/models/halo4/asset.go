package halo4

// Asset defines the structure of an Asset
type Asset struct {
	// BaseURL is the key to match the AssetURL to a metadata prefix.
	BaseURL string `json:"baseUrl" bson:"baseUrl"`

	// AssetURL is the suffix path of the asset.
	AssetURL string `json:"assetUrl" bson:"assetUrl"`
}
