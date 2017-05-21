package response

// Options defines the structure of the Service Record response.
type Options struct {
	Response `bson:",inline"`

	// Identifier is the identifier of the Halo Waypoint web app.
	Identifier string `json:"identifier" bson:"identifier"`

	// ServiceList is a list of service endpoints.
	ServiceList map[string]string `json:"serviceList" bson:"serviceList"`

	// Settings is a list of application dependant settings.
	Settings map[string]string `json:"settings" bson:"settings"`
}
