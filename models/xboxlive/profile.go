package xboxlive

type ProfileUsers struct {
	Response

	Users []ProfileUser `json:"profileUsers"`
}

type ProfileUser struct {
	XUID            string               `json:"id"`
	HostID          string               `json:"hostId"`
	Settings        []ProfileUserSetting `json:"settings"`
	IsSponsoredUser bool                 `json:"isSponsoredUser"`
}

type ProfileUserSetting struct {
	ID    string `json:"id"`
	Value string `json:"value"`
}
