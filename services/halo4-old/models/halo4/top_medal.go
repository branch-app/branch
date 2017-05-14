package halo4

// TopMedal defines the structure of Service Record Top Medals.
type TopMedal struct {
	// ID is the ID of the medal.
	ID uint `json:"id" bson:"id"`

	// Name is the name of the medal.
	Name string `json:"name" bson:"name"`

	// Description is the description of the medal.
	Description string `json:"description" bson:"description"`

	// ImageURL is the asset of the medal.
	ImageURL Asset `json:"imageUrl" bson:"imageUrl"`

	// TotalMedals is the total number of times this medal has been earned by the player.
	TotalMedals uint `json:"totalMedals" bson:"totalMedals"`
}
