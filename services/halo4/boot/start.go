package boot

import (
	"os"

	"encoding/json"

	"github.com/branch-app/branch-mono-go/clients/auth"
	"github.com/branch-app/branch-mono-go/clients/xboxlive"
	"github.com/branch-app/branch-mono-go/services/halo4/app"
	"github.com/branch-app/branch-mono-go/services/halo4/models"
	"github.com/branch-app/branch-mono-go/services/halo4/server"
	"github.com/branch-app/branch-mono-go/services/halo4/services/waypoint"
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

	authClient := auth.NewClient(config.AuthURL, config.AuthURL)
	xboxliveClient := xboxlive.NewClient(config.XboxLiveURL, config.XboxLiveKey)
	waypointService := waypoint.NewClient(authClient, config.Mongo, "branch_service-halo4")

	app := app.NewApp(authClient, xboxliveClient, waypointService)
	server := server.NewServer(app)
	server.Run(addr)
}
