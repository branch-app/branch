package response

type ProfileUsers struct {
	Base `bson:",inline"`

	Users []ProfileUser `json:"profileUsers" bson:"profileUsers"`
}

type ProfileUser struct {
	XUID            string               `json:"id" bson:"xuid"`
	HostID          string               `json:"hostId" bson:"hostId"`
	Settings        []ProfileUserSetting `json:"settings" bson:"settings"`
	IsSponsoredUser bool                 `json:"isSponsoredUser" bson:"isSponsoredUser"`
}

type ProfileUserSetting struct {
	ID    string `json:"id" bson:"id"`
	Value string `json:"value" bson:"value"`
}
