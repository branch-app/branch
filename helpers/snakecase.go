package helpers

import "unicode"

// StrToSnake converts a string to it's snake_case representation. Code was taken from
// https://gist.github.com/elwinar/14e1e897fdbe4d3432e1
func StrToSnake(str string) string {
	runes := []rune(str)
	length := len(runes)

	var out []rune
	for i := 0; i < length; i++ {
		if i > 0 && unicode.IsUpper(runes[i]) && ((i+1 < length && unicode.IsLower(runes[i+1])) || unicode.IsLower(runes[i-1])) {
			out = append(out, '_')
		}
		out = append(out, unicode.ToLower(runes[i]))
	}

	return string(out)
}

// StrToSnakeTrimmed converts a string to it's snake_case representation, and trims the
// result to the given length Code was taken from:
// https://gist.github.com/elwinar/14e1e897fdbe4d3432e1
func StrToSnakeTrimmed(str string, trimLength int) string {
	str = StrToSnake(str)

	if len(str) <= trimLength {
		return str
	}

	return str[:trimLength]
}
