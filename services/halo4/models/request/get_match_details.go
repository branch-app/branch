package request

type GetMatchDetails struct {
	MatchID string `json:"matchId" bson:"matchId"`
}
