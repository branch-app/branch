package main

import (
	"net/http"

	"github.com/branch-app/log-go"
	"github.com/branch-app/service-xboxlive/clients"
	"github.com/branch-app/service-xboxlive/contexts"
	"github.com/branch-app/service-xboxlive/handlers"
	"github.com/branch-app/service-xboxlive/models"
	sharedClients "github.com/branch-app/shared-go/clients"

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

	// Create service context
	ctx := &contexts.ServiceContext{
		//ServiceID:     "service-xboxlive",
		HTTPClient:     sharedClients.NewHTTPClient(),
		ServiceClient:  sharedClients.NewServiceClient(),
		XboxLiveClient: clients.NewXboxLiveClient(config.MongoDB),
		Configuration:  &config,
	}

	// Create Gin
	r := gin.Default()
	apiGroup := r.Group("v1/")
	{
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
	branchlog.Info("service_listening", nil, &map[string]interface{}{"port": config.Port})
	r.Run(fmt.Sprintf(":%s", config.Port))
}

type m struct {
	data *branchlog.BranchError
}
type n struct {
	data branchlog.BranchError
}
