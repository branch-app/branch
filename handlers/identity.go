package handlers

import (
	"net/http"

	"github.com/branch-app/log-go"
	"github.com/branch-app/service-xboxlive/contexts"
	"github.com/branch-app/service-xboxlive/helpers"
	"github.com/branch-app/service-xboxlive/models/branch"
	"gopkg.in/gin-gonic/gin.v1"
)

type IdentityHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl IdentityHandler) Get(c *gin.Context) {
	identityCall, err := helpers.ParseIdentity(c.Param("identity"))
	if err != nil {
		c.JSON(http.StatusNotAcceptable, branchlog.NewBranchErrorFromError(err, nil, nil))
		return
	}

	identity, err := hdl.ctx.XboxLiveClient.GetProfileIdentity(identityCall)
	if err != nil {
		c.JSON(hdl.ctx.XboxLiveClient.ErrorToHTTPStatus(err), branchlog.NewBranchErrorFromError(err, nil, nil))
		return
	}

	resp := &branch.Response{
		CacheDate: identity.CachedAt,
		Data:      identity,
	}
	c.JSON(http.StatusOK, resp)
}

func NewIdentityHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *IdentityHandler {
	hdl := &IdentityHandler{}
	hdl.ctx = ctx

	rg = rg.Group("identity")
	rg.GET("/:identity", hdl.Get)

	return hdl
}
