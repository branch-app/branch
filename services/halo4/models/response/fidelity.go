package response

// Fidelity defines how accurate a field is.
type Fidelity int

const (
	// LowFidelity is used when a value is completely private.
	LowFidelity Fidelity = 0

	// MildFidelity is used when a value partially private.
	MildFidelity Fidelity = 1

	// HighFidelity is used when a value isn't private.
	HighFidelity Fidelity = 2
)
