package handlers

import (
	"net/http"

	"github.com/TheTree/service-xboxlive/contexts"
	"gopkg.in/gin-gonic/gin.v1"
)

type XUIDHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl XUIDHandler) Get(c *gin.Context) {
	xuid := c.Param("xuid")
	identity := hdl.ctx.XboxLiveStore.GetByXUID(xuid)

	if identity != nil {
		c.JSON(http.StatusOK, identity)
	} else {
		c.Status(http.StatusNotFound)
	}
}

func NewXUIDHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *XUIDHandler {
	hdl := &XUIDHandler{}
	hdl.ctx = ctx

	rg = rg.Group("xuid")
	rg.GET("/:xuid", hdl.Get)

	return hdl
}
