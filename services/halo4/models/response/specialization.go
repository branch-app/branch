package response

// Specialization defines the structure of Service Record Specializations.
type Specialization struct {
	// ID is the id of the specialization.
	ID uint `json:"id" bson:"id"`

	// Name is the name of the specialization.
	Name string `json:"name" bson:"name"`

	// Description is the description of the specialization.
	Description string `json:"description" bson:"description"`

	// ImageURL is the asset of the specialization.
	ImageURL Asset `json:"imageUrl" bson:"imageUrl"`

	// Level is the current level reached in the specialization.
	Level uint `json:"level" bson:"level"`

	// LevelName is the name of the current level of the specialization.
	LevelName string `json:"levelName" bson:"levelName"`

	// PercentComplete is how complete this specialization is.
	PercentComplete float64 `json:"percentComplete" bson:"percentComplete"`

	// IsCurrent defines if the player is current working on completing this
	// specialization.
	IsCurrent bool `json:"isCurrent" bson:"isCurrent"`

	// Completed defines if the specialization is completed.
	Completed bool `json:"completed" bson:"completed"`
}
