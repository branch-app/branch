package handlers

import (
	"net/http"

	"github.com/TheTree/service-xboxlive/contexts"
	"gopkg.in/gin-gonic/gin.v1"
)

type GamertagHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl GamertagHandler) Get(c *gin.Context) {
	gamertag := c.Param("gamertag")
	identity := hdl.ctx.XboxLiveStore.GetByGT(gamertag)

	if identity != nil {
		c.JSON(http.StatusOK, identity)
	} else {
		c.Status(http.StatusNotFound)
	}
}

func NewGamertagHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *GamertagHandler {
	hdl := &GamertagHandler{}
	hdl.ctx = ctx

	rg = rg.Group("gamertag")
	rg.GET("/:gamertag", hdl.Get)

	return hdl
}
