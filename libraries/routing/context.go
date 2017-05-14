package routing

import (
	"gopkg.in/gin-gonic/gin.v1"
)

// Context contains the data required to process a request
type Context struct {
	App        interface{}
	GinContext *gin.Context
}
