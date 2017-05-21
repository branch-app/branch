package response

// Season defines the structure of a Spartan Ops Season
type Season struct {
	// ID is the identifier of the season.
	ID int `json:"id" bson:"id"`

	// Episodes is the list of episodes inside the season
	Episodes []Episode `json:"episodes" bson:"episodes"`

	// Epilogue is the epilogue episode.
	Epilogue Episode `json:"epilogue" bson:"epilogue"`
}

// Episode defines the structure of a Spartan Ops Season's Episode
type Episode struct {
	// ID is the identifier of the episode.
	ID int `json:"id" bson:"id"`

	// Title is the friendly identifier of the episode.
	Title string `json:"title" bson:"title"`

	// Description is the description of the episode.
	Description string `json:"description" bson:"description"`

	// Videos is a list of the videos in an episode.
	Videos []EpisodeVideo `json:"videos" bson:"videos"`

	// Chapters is a list of chapters in the episode.
	Chapters []EpisodeChapter `json:"chapters" bson:"chapters"`

	// ImageURL is an asset of the episode.
	ImageURL Asset `json:"imageUrl" bson:"imageUrl"`
}

// EpisodeVideo defines the structure of a Spartan Ops Episode's video.
type EpisodeVideo struct {
	// Language is the language of the video.
	Language string `json:"language" bson:"language"`

	// Folder is the folder the video is in.
	Folder string `json:"folder" bson:"folder"`

	// WebFileName is the filename for the Web encoded video.
	WebFileName string `json:"webFileName" bson:"webFileName"`

	// ConsoleFileName is the filename for the Console encoded video.
	ConsoleFileName string `json:"consoleFileName" bson:"consoleFileName"`

	// MobileFileName is the filename for the Mobile encoded video.
	MobileFileName string `json:"mobileFileName" bson:"mobileFileName"`

	// IosFileName is the filename for the iOS encoded video.
	IosFileName string `json:"iosFileName" bson:"iosFileName"`

	// IosSuffix is the suffix for the iOS file.
	IosSuffix string `json:"iosSuffix" bson:"iosSuffix"`

	// MP4FileName is the filename for the mp4 encoded video.
	MP4FileName string `json:"mp4FileName" bson:"mp4FileName"`

	// MP4Suffix is the suffix for the MP4 file.
	MP4Suffix string `json:"mp4Suffix" bson:"mp4Suffix"`
}

// EpisodeChapter defines the structure of a Spartan Ops Episode Chapter.
type EpisodeChapter struct {
	// ID is the identifier of the chapter.
	ID int `json:"id" bson:"id"`

	// Number is the index of the chapter.
	Number int `json:"number" bson:"number"`

	// Title is the friendly identifier of the chapter.
	Title string `json:"title" bson:"title"`

	// Description is the description of the chapter.
	Description string `json:"description" bson:"description"`

	// ImageURL is an asset of the chapter.
	ImageURL Asset `json:"imageUrl" bson:"imageUrl"`
}
