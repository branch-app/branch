package response

// MissionCompletion defines the structure of a Mission's Completion State.
type MissionCompletion struct {
	// MapID is the identifer of the Map.
	MapID uint `json:"mapId" bson:"mapId"`

	// Mission is the index of the mission (1 indexed)
	Mission uint `json:"mission" bson:"mission"`

	// Difficulty is the highest Difficulty Level the mission has been completed on.
	Difficulty DifficultyLevel `json:"difficulty" bson:"difficulty"`
}
