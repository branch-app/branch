package helpers

import "errors"

// ByteSliceInsert inserts byte slice b into a, at n.
func ByteSliceInsert(a, b []byte, n int) ([]byte, error) {
	if n > len(a) {
		return nil, errors.New("n is greater than length of a")
	}

	merged := make([]byte, 0)
	merged = append(merged, a[0:n]...)
	merged = append(merged, b...)
	merged = append(merged, a[n:len(a)]...)

	return merged, nil
}
