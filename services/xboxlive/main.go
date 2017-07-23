package main

import (
	"fmt"
	"os"

	"github.com/branch-app/branch-mono-go/services/xboxlive/boot"
)

var commands = map[string]func(){
	"debug": boot.RunDebug,
	"start": boot.RunStart,
}

func main() {
	if len(os.Args) == 1 {
		fmt.Println("usage: service-admin <command>")
		fmt.Println("The valid commands are:")
		for key := range commands {
			fmt.Printf("  %s\n", key)
		}
		return
	}

	cmd := commands[os.Args[1]]

	if cmd == nil {
		fmt.Printf("%q is not valid command.\n", os.Args[1])
		os.Exit(2)
		return
	}

	cmd()
}
