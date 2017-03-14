package main

import (
	"net/http"
	"os"

	log "github.com/branch-app/log-go"
	"github.com/branch-app/service-xboxlive/clients"
	"github.com/branch-app/service-xboxlive/contexts"
	"github.com/branch-app/service-xboxlive/handlers"
	"github.com/branch-app/service-xboxlive/models"
	sharedClients "github.com/branch-app/shared-go/clients"
	"github.com/branch-app/shared-go/types"

	"fmt"

	"github.com/jinzhu/configor"
	"gopkg.in/gin-gonic/gin.v1"
)

func init() {

}

func main() {
	// Load Config
	var config models.Configuration
	configor.Load(&config, "config.json")

	// Load Environment
	env := types.StrToEnvironment(os.Getenv("BRANCH_ENVIRONMENT"))

	// Create service context
	ctx := &contexts.ServiceContext{
		//ServiceID:     "service-xboxlive",
		HTTPClient:     sharedClients.NewHTTPClient(),
		ServiceClient:  sharedClients.NewServiceClient(env),
		XboxLiveClient: clients.NewXboxLiveClient(env, config.MongoDB),
		Configuration:  &config,
	}

	// Create Gin
	r := gin.Default()
	apiGroup := r.Group("v1/")
	{
		handlers.NewAssetsHandler(apiGroup, ctx)
		handlers.NewIdentityHandler(apiGroup, ctx)
		handlers.NewProfileHandler(apiGroup, ctx)
	}

	// Init health check
	r.GET("/system/health", func(c *gin.Context) {
		c.JSON(http.StatusOK, gin.H{
			"status": "healthy",
		})
	})

	// Start Service
	log.Info("service_listening", nil, &log.M{"port": config.Port})
	r.Run(fmt.Sprintf(":%s", config.Port))
}
