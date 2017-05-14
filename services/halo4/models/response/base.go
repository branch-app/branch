package response

import "github.com/branch-app/branch-mono-go/domain/branch"

type Response struct {
	branch.Response `bson:",inline"`

	// StatusCode defines the status of the response.
	StatusCode StatusCode `json:"statusCode" bson:"statusCode"`

	// StatusReason is the reason the response failed if the StatusCode is not 1.
	StatusReason string `json:"statusReason" bson:"statusReason"`

	// DateFidelity defines the levels of fidelity of FirstPlayed and LastPlayed.
	DateFidelity Fidelity `json:"dateFidelity" bson:"dateFidelity"`
}
