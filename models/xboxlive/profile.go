package xboxlive

type ProfileUsers struct {
	Response `bson:",inline"`

	Users []ProfileUser `json:"profileUsers" bson:"profile_users"`
}

type ProfileUser struct {
	XUID            string               `json:"id" bson:"xuid"`
	HostID          string               `json:"hostId" bson:"host_id"`
	Settings        []ProfileUserSetting `json:"settings" bson:"settings"`
	IsSponsoredUser bool                 `json:"isSponsoredUser" bson:"is_sponsored_user"`
}

type ProfileUserSetting struct {
	ID    string `json:"id" bson:"id"`
	Value string `json:"value" bson:"value"`
}
