package handlers

import (
	"net/http"

	"github.com/branch-app/service-xboxlive/contexts"
	"github.com/branch-app/service-xboxlive/helpers"
	"gopkg.in/gin-gonic/gin.v1"
)

type ProfileHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl ProfileHandler) Settings(c *gin.Context) {
	identityCall, _ := helpers.ParseIdentity(c.Param("identity"))

	identity, _ := hdl.ctx.XboxLiveClient.GetProfileIdentity(identityCall)
	profileSettings, _ := hdl.ctx.XboxLiveClient.GetProfileSettings(identity)
	c.JSON(http.StatusOK, &profileSettings)
}

func NewProfileHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *ProfileHandler {
	hdl := &ProfileHandler{}
	hdl.ctx = ctx

	rg = rg.Group("profile")
	rg.GET("/:identity/settings", hdl.Settings)

	return hdl
}
