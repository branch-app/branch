package response

// MatchResult is an enum representing Halo 4 Match Results.
type MatchResult int

const (
	MatchResultUnknown MatchResult = -1
	MatchResultLost    MatchResult = 0
	MatchResultDraw    MatchResult = 1
	MatchResultWon     MatchResult = 2
)
