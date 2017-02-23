package handlers

import (
	"github.com/TheTree/service-auth/contexts"
	"gopkg.in/gin-gonic/gin.v1"
)

type XboxLiveHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl XboxLiveHandler) Get(c *gin.Context) {

}

func NewXboxLiveHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *XboxLiveHandler {
	hdl := &XboxLiveHandler{}
	hdl.ctx = ctx

	rg = rg.Group("xboxlive")
	rg.GET("/", hdl.Get)

	return hdl
}
