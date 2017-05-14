package log

import (
	"encoding/json"
	goLog "log"
	"os"
)

var (
	logOut = goLog.New(os.Stdout, "", 0)
	logErr = goLog.New(os.Stderr, "", 0)
)

type Level int

const (
	LevelDebug Level = iota
	LevelInfo
	LevelWarn
	LevelError
)

func Debug(code string, meta M, reasons ...interface{}) *E {
	err := newBranchError(code, meta, reasons)
	go handleError(LevelDebug, *err)
	return err
}

func Info(code string, meta M, reasons ...interface{}) *E {
	err := newBranchError(code, meta, reasons)
	go handleError(LevelInfo, *err)
	return err
}

func Warn(code string, meta M, reasons ...interface{}) *E {
	err := newBranchError(code, meta, reasons)
	go handleError(LevelWarn, *err)
	return err
}

func Error(code string, meta M, reasons ...interface{}) *E {
	err := newBranchError(code, meta, reasons)
	go handleError(LevelError, *err)
	return err
}

func handleError(level Level, err E) {
	msg := "failed to marshal error"

	if bytes, err := json.Marshal(err); err == nil {
		msg = string(bytes)
	}

	switch level {
	case LevelDebug:
		logOut.Printf("debug:%s\n", msg)
	case LevelInfo:
		logOut.Printf("info:%s\n", msg)
	case LevelWarn:
		logErr.Printf("warn:%s\n", msg)
	case LevelError:
		logErr.Printf("error:%s\n", msg)
	}
}
