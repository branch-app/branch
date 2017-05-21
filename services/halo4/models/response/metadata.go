package response

// Metadata defines the structure of the Metadata response.
type Metadata struct {
	Response `bson:",inline"`

	// DifficultiesMetadata contains the Difficulty Level metadata.
	DifficultiesMetadata *DifficultiesMetadata `json:"difficultiesMetadata,omitempty" bson:"difficultiesMetadata"`

	// SpartanOpsMetadata
	SpartanOpsMetadata *SpartanOpsMetadata `json:"spartanOpsMetadata,omitempty" bson:"spartanOpsMetadata"`
}

// DifficultiesMetadata defines the structure of the Difficulties metadata response.
type DifficultiesMetadata struct {
	// Difficulties is an array of all the difficulty levels.
	Difficulties []Difficulty `json:"difficulties" bson:"difficulties"`
}

// SpartanOpsMetadata defines the structure of the Spartan Ops metadata response.
type SpartanOpsMetadata struct {
	// SeasonsReleasedToDate holds the count of the currently released seasons.
	SeasonsReleasedToDate int `json:"seasonsReleasedToDate" bson:"seasonsReleasedToDate"`

	// ChaptersCurrentlyAvailable holds the count of the available chapters.
	ChaptersCurrentlyAvailable int `json:"chaptersCurrentlyAvailable" bson:"chaptersCurrentlyAvailable"`

	// BumperType unk
	BumperType int `json:"bumperType" bson:"bumperType"`

	// CurrentSeason holds the current active season.
	CurrentSeason int `json:"currentSeason" bson:"currentSeason"`

	// CurrentEpisode holds the current active episode.
	CurrentEpisode int `json:"currentEpisode" bson:"currentEpisode"`

	// Seasons holds a list of Spartan Ops seasons.
	Seasons []Season `json:"seasons" bson:"seasons"`
}
