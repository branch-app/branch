package handlers

import (
	"net/http"

	log "github.com/branch-app/log-go"
	"github.com/branch-app/service-xboxlive/contexts"
	"gopkg.in/gin-gonic/gin.v1"
)

type AssetsHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl AssetsHandler) Get(c *gin.Context) {
	xblc := hdl.ctx.XboxLiveClient
	asset, err := xblc.GetColourAssets(c.Param("colourID"))
	if err != nil {
		c.JSON(xblc.ErrorToHTTPStatus(err), log.Error(err.Error(), nil, nil))
		return
	}

	c.JSON(http.StatusOK, &asset)
}

func NewAssetsHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *AssetsHandler {
	hdl := &AssetsHandler{}
	hdl.ctx = ctx

	rg = rg.Group("assets")
	rg.GET("/colours/:colourID", hdl.Get)

	return hdl
}
