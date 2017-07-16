package response

// Commendations defines the structure of the Commendations response.
type Commendations struct {
	Response `bson:",inline"`

	// Commendations contains the details of the players commendation progression.
	Commendations []Commendation `json:"commendations" bson:"commendations"`
}

// Commendation defines the structure of a commendation.
type Commendation struct {
	// ID is the identifier of the commendation.
	ID int `json:"id" bson:"id"`

	// Name is the name of the commendations.
	Name string `json:"name" bson:"name"`

	// Description is the description of the commendations.
	Description string `json:"description" bson:"description"`

	// CategoryID is the identifier of the category of the commendations.
	CategoryID int `json:"categoryId" bson:"categoryId"`

	// CategoryName is the name of the category of the commendations.
	CategoryName string `json:"categoryName" bson:"categoryName"`

	// Ticks is the number of times the commendation has been triggered.
	Ticks int `json:"ticks" bson:"ticks"`

	// LevelID is the identifier of the level of the commendations.
	LevelID int `json:"levelId" bson:"levelId"`

	// LevelName is the name of the level of the commendations.
	LevelName *string `json:"levelName" bson:"levelName"`

	// LevelStartTicks is the number of ticks required to start the current level of the
	// commendations.
	LevelStartTicks int `json:"levelStartTicks" bson:"levelStartTicks"`

	// NextLevel holds information about the next level of the commendation.
	NextLevel CommendationNextLevel `json:"nextLevel" bson:"nextLevel"`
}

// CommendationNextLevel defines the structure the next commendation level.
type CommendationNextLevel struct {
	// NextLevelName is the name of the next level of the commendation.
	NextLevelName *string `json:"nextLevelName" bson:"nextLevelName"`

	// ProgressToNextLevel is the progression to the next commendation level.
	ProgressToNextLevel float64 `json:"progressToNextLevel" bson:"progressToNextLevel"`

	// NextLevelStartTicks is the tick count to start the next commendation level.
	NextLevelStartTicks int `json:"nextLevelStartTicks" bson:"nextLevelStartTicks"`
}
