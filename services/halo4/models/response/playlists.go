package response

// Playlists defines the structure of the Playlists response.
type Playlists struct {
	Response `bson:",inline"`

	// Playlists contains the details of the global Halo 4 playlists.
	Playlists []Playlist `json:"playlists" bson:"playlists"`
}

// Playlist defines the structure of a playlist in the global Halo 4 playlists.
type Playlist struct {
	ID              int                   `json:"id" bson:"id"`
	IsCurrent       bool                  `json:"isCurrent" bson:"isCurrent"`
	PopulationCount int                   `json:"populationCount" bson:"populationCount"`
	Name            string                `json:"name" bson:"name"`
	Description     string                `json:"description" bson:"description"`
	ModeID          int                   `json:"modeId" bson:"modeId"`
	ModeName        string                `json:"modeName" bson:"modeName"`
	MaxPartySize    int                   `json:"maxPartySize" bson:"maxPartySize"`
	MaxLocalPlayers int                   `json:"maxLocalPlayers" bson:"maxLocalPlayers"`
	IsFreeForAll    bool                  `json:"isFreeForAll" bson:"isFreeForAll"`
	ImageURL        Asset                 `json:"imageUrl" bson:"imageUrl"`
	GameVariants    []PlaylistGameVariant `json:"gameVariants" bson:"gameVariants"`
	MapVariants     []PlaylistMapVariant  `json:"mapVariants" bson:"mapVariants"`
}

// PlaylistGameVariant defines the structure of a playlist game variant.
type PlaylistGameVariant struct {
	// GameVariantName is the name of the game variant.
	GameVariantName string `json:"gameVariantName" bson:"gameVariantName"`

	// GameVariantDescription is the description of the variant.
	GameVariantDescription string `json:"gameVariantDescription" bson:"gameVariantDescription"`

	// GameBaseVariantID is the ID of the base game variant.
	GameBaseVariantID int `json:"gameBaseVariantId" bson:"gameBaseVariantId"`

	// GameBaseVariantName is the name of the base game variant.
	GameBaseVariantName string `json:"gameBaseVariantName" bson:"gameBaseVariantName"`

	// GameBaseVariantDescription is the description of the base game variant.
	GameBaseVariantDescription string `json:"gameBaseVariantDescription" bson:"gameBaseVariantDescription"`

	// GameBaseVariantImageURL is the a
	GameBaseVariantImageURL Asset `json:"gameBaseVariantImageUrl" bson:"gameBaseVariantImageUrl"`
}

// PlaylistMapVariant defines the structure of a playlist map variant.
type PlaylistMapVariant struct {
	// MapID is the id of the map in the playlist.
	MapID int `json:"mapId" bson:"mapId"`

	// MapVariantName is the name of the map in the playlist.
	MapVariantName string `json:"mapVariantName" bson:"mapVariantName"`
}
