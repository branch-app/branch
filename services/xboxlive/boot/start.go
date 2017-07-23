package boot

import (
	"os"

	"encoding/json"

	"github.com/branch-app/branch-mono-go/clients/auth"
	"github.com/branch-app/branch-mono-go/services/xboxlive/app"
	"github.com/branch-app/branch-mono-go/services/xboxlive/models"
	"github.com/branch-app/branch-mono-go/services/xboxlive/server"
	"github.com/branch-app/branch-mono-go/services/xboxlive/services/xboxlive"
)

func RunStart() {
	// Load config
	var config models.Config
	if err := json.Unmarshal([]byte(os.Getenv("CONFIG")), &config); err != nil {
		panic(err)
	}

	// Set port from env, otherwise default
	addr := os.Getenv("ADDR")
	if addr == "" && config.Addr != "" {
		addr = config.Addr
	} else if addr == "" {
		addr = "localhost:3000"
	}

	auth := auth.NewClient(config.AuthURL, config.AuthURL)
	xblService := xboxlive.NewClient(auth, config.Mongo, "branch_service-xboxlive")

	app := app.NewApp(auth, xblService)
	server := server.NewServer(app)
	server.Run(addr)
}
