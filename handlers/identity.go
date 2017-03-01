package handlers

import (
	"net/http"

	"github.com/branch-app/service-xboxlive/contexts"
	"github.com/branch-app/service-xboxlive/helpers"
	"github.com/branch-app/service-xboxlive/models/branch"
	"gopkg.in/gin-gonic/gin.v1"
)

type IdentityHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl IdentityHandler) Get(c *gin.Context) {
	identityCall := helpers.ParseIdentity(c.Param("identity"))
	if identityCall == nil {
		panic("identity_not_found")
	}

	xblc := hdl.ctx.XboxLiveClient
	identity, err := xblc.GetProfileIdentity(identityCall)
	if err != nil {
		panic(err)
	}

	resp := branch.NewResponse(identity.CachedAt, identity)
	c.JSON(http.StatusOK, resp)
}

func NewIdentityHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *IdentityHandler {
	hdl := &IdentityHandler{}
	hdl.ctx = ctx

	rg = rg.Group("identity")
	rg.GET("/:identity", hdl.Get)

	return hdl
}
