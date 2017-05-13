package halo4

// SkillRank defines the structure of Service Record Skill Ranks.
type SkillRank struct {
	// PlaylistName is the name of the playlist related to this skill rank.
	PlaylistName string `json:"playlistName" bson:"playlistName"`

	// PlaylistDescription is the description of the playlist related to this skill rank.
	PlaylistDescription string `json:"playlistDescription" bson:"playlistDescription"`

	// PlaylistImageURL is the asset for the
	PlaylistImageURL Asset `json:"playlistImageUrl" bson:"playlistImageUrl"`

	// PlaylistID is the ID of the playlist related to this skill rank.
	PlaylistID int `json:"playlistId" bson:"playlistId"`

	// CurrentSkillRank is the current rank of the player in this playlist. Null if no
	// skill rank has been assigned.
	CurrentSkillRank *int `json:"currentSkillRank" bson:"currentSkillRank"`
}
