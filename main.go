package main

import (
	"net/http"

	"github.com/TheTree/service-auth/contexts"
	"github.com/TheTree/service-auth/handlers"
	"gopkg.in/gin-gonic/gin.v1"
)

func init() {

}

func main() {
	// Create service context
	ctx := &contexts.ServiceContext{
	// ServiceContext.ServiceID: "",
	// ServiceID:                "service-auth",
	}

	// Create Gin
	r := gin.Default()
	apiGroup := r.Group("v1/")
	{
		handlers.NewXboxLiveHandler(apiGroup, ctx)
	}

	// Init health check
	r.GET("/system/health", func(c *gin.Context) {
		c.JSON(http.StatusOK, gin.H{
			"status": "healthy",
		})
	})
	r.Run(":3000")
}
