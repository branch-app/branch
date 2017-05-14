package server

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"
	"github.com/branch-app/branch-mono-go/libraries/routing"
	"github.com/branch-app/branch-mono-go/services/halo4/app"

	"gopkg.in/gin-gonic/gin.v1"
)

// Server uses an App, but exposes its functionality via HTTP
type Server struct {
	app    app.App
	engine *gin.Engine
}

// NewServer creates a Server instance with an App instance
func NewServer(app app.App) *Server {
	engine := gin.New()

	engine.GET("/system/health", func(c *gin.Context) {
		c.Status(http.StatusNoContent)
	})

	engine.POST("/1/:version/:method", func(c *gin.Context) {
		routing.RouteRequest(c, app)
	})

	return &Server{
		app:    app,
		engine: engine,
	}
}

// Run the Server and listen on the specified address
func (s *Server) Run(addr string) {
	log.Info("http_listening", log.M{"addr": addr})
	if err := s.engine.Run(addr); err != nil {
		log.Warn("service_exited", log.M{"service": "service-xboxlive"}, err)
	}
}

func healthCheck(c *gin.Context) {
	c.Status(http.StatusNoContent)
}
