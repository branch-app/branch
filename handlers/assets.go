package handlers

import (
	"github.com/branch-app/service-xboxlive/contexts"
	"gopkg.in/gin-gonic/gin.v1"
)

type AssetsHandler struct {
	ctx *contexts.ServiceContext
}

// func (hdl AssetsHandler) Get(c *gin.Context) {
// 	xblc := hdl.ctx.XboxLiveClient
// 	asset, err := xblc.GetColourAssets(c.Param("colourID"))
// 	if err != nil {
// 		bErr := log.Error(err.Error(), nil, nil)
// 		c.JSON(xblc.ErrorToHTTPStatus(err), &bErr)
// 		return
// 	}

// 	c.JSON(http.StatusOK, &asset)
// }

func NewAssetsHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *AssetsHandler {
	hdl := &AssetsHandler{}
	hdl.ctx = ctx

	rg = rg.Group("assets")
	// rg.GET("/colours/:colourID", hdl.Get)

	return hdl
}
