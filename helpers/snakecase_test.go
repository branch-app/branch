package helpers

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

// This was taken from https://gist.github.com/elwinar/14e1e897fdbe4d3432e1
var tests = map[string]string{
	"a":             "a",
	"snake":         "snake",
	"A":             "a",
	"ID":            "id",
	"MOTD":          "motd",
	"Snake":         "snake",
	"SnakeTest":     "snake_test",
	"SnakeID":       "snake_id",
	"SnakeIDGoogle": "snake_id_google",
	"LinuxMOTD":     "linux_motd",
	"OMGWTFBBQ":     "omgwtfbbq",
	"omg_wtf_bbq":   "omg_wtf_bbq",
}

// Modified from https://gist.github.com/elwinar/14e1e897fdbe4d3432e1
var testsTrimmed = map[string]string{
	"a":             "a",
	"snake":         "snake",
	"A":             "a",
	"ID":            "id",
	"MOTD":          "motd",
	"Snake":         "snake",
	"SnakeTest":     "snake_te",
	"SnakeID":       "snake_id",
	"SnakeIDGoogle": "snake_id",
	"LinuxMOTD":     "linux_mo",
	"OMGWTFBBQ":     "omgwtfbb",
	"omg_wtf_bbq":   "omg_wtf_",
}

func TestStrToSnake(t *testing.T) {
	for a, r := range tests {
		result := StrToSnake(a)
		assert.Equal(t, r, result)
	}
}

func TestStrToSnakeTrimmed(t *testing.T) {
	for a, r := range testsTrimmed {
		result := StrToSnakeTrimmed(a, 8)
		assert.Equal(t, r, result)
	}
}
