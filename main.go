package main

import (
	"net/http"

	"github.com/TheTree/service-xboxlive/contexts"
	"github.com/TheTree/service-xboxlive/handlers"
	"github.com/TheTree/service-xboxlive/helpers"
	"gopkg.in/gin-gonic/gin.v1"
)

func init() {

}

func main() {
	// Create service context
	ctx := &contexts.ServiceContext{
		ServiceID:     "service-xboxlive",
		XboxLiveStore: helpers.NewXboxLiveStore(),
	}

	// Create Gin
	r := gin.Default()
	apiGroup := r.Group("v1/")
	{
		handlers.NewXUIDHandler(apiGroup, ctx)
		handlers.NewGamertagHandler(apiGroup, ctx)
	}

	// Init health check
	r.GET("/system/health", func(c *gin.Context) {
		c.JSON(http.StatusOK, gin.H{
			"status": "healthy",
		})
	})
	r.Run(":3000")
}
