package handlers

import (
	"net/http"

	"github.com/branch-app/service-xboxlive/contexts"
	"github.com/branch-app/service-xboxlive/models"
	"gopkg.in/gin-gonic/gin.v1"
)

type ProfileHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl ProfileHandler) Settings(c *gin.Context) {
	xuid := c.Param("xuid")

	identity := hdl.ctx.XboxLiveClient.GetProfileIdentity(&models.IdentityCall{Identity: xuid, Type: "xuid"})
	profileSettings := hdl.ctx.XboxLiveClient.GetProfileSettings(identity)
	c.JSON(http.StatusOK, profileSettings)
}

func NewProfileHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *ProfileHandler {
	hdl := &ProfileHandler{}
	hdl.ctx = ctx

	rg = rg.Group("profile")
	rg.GET("/:xuid/settings", hdl.Settings)

	return hdl
}
