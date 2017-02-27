package handlers

import (
	"net/http"

	"strings"

	"github.com/branch-app/log-go"
	"github.com/branch-app/service-xboxlive/contexts"
	"github.com/branch-app/service-xboxlive/models"
	"gopkg.in/gin-gonic/gin.v1"
)

type IdentityHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl IdentityHandler) Get(c *gin.Context) {
	identityType := strings.ToLower(c.Param("type"))
	identityValue := strings.ToLower(c.Param("value"))
	identityCall := &models.IdentityCall{
		Identity: identityValue,
		Type:     identityType,
	}

	if identityCall.Type != "xuid" && identityCall.Type != "gamertag" {
		c.JSON(http.StatusNotAcceptable, branchlog.NewBranchError("invalid_identity_type", nil, nil))
		return
	}

	identity := hdl.ctx.XboxLiveClient.GetProfileIdentity(identityCall)
	c.JSON(http.StatusOK, identity)
}

func NewIdentityHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *IdentityHandler {
	hdl := &IdentityHandler{}
	hdl.ctx = ctx

	rg = rg.Group("identity")
	rg.GET("/:type/:value", hdl.Get)

	return hdl
}
