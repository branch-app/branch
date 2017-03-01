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
	identityCall := helpers.ParseIdentity(c.Param("identity"))
	if identityCall == nil {
		// pass into handler
		panic("identity_not_found")
	}

	xmlc := hdl.ctx.XboxLiveClient
	identity, err := xmlc.GetProfileIdentity(identityCall)
	if err != nil {
		panic(err)
	}

	profileSettings, err := xmlc.GetProfileSettings(identity)
	if err != nil {
		panic(err)
	}
	c.JSON(http.StatusOK, &profileSettings)
}

func NewProfileHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *ProfileHandler {
	hdl := &ProfileHandler{}
	hdl.ctx = ctx

	rg = rg.Group("profile")
	rg.GET("/:identity/settings", hdl.Settings)

	return hdl
}
