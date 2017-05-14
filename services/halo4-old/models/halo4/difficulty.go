package halo4

// Difficulty defines the structure of a Halo 4 Difficulty Level.
type Difficulty struct {
	// ID is the identifier of the Difficulty Level.
	ID DifficultyLevel `json:"id" bson:"id"`

	// Name is the name of the Difficulty Level.
	Name string `json:"name" bson:"name"`

	// Description is the description of the Difficulty Level.
	Description string `json:"description" bson:"description"`

	// ImageURL is the asset of the Difficulty Level.
	ImageURL Asset `json:"imageUrl" bson:"imageUrl"`
}
