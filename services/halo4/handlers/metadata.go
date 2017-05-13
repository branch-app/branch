package handlers

import (
	"net/http"

	log "github.com/branch-app/log-go"
	"github.com/branch-app/service-halo4/contexts"

	"gopkg.in/gin-gonic/gin.v1"
)

type MetadataHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl MetadataHandler) GetMetadata(c *gin.Context) {
	typeStr := c.DefaultQuery("type", "")
	h4c := hdl.ctx.Halo4Client
	metadata, err := h4c.GetMetadataWithType(typeStr)
	if err != nil {
		c.JSON(h4c.ErrorToHTTPStatus(err), log.Error(err.Error(), nil, nil))
		return
	}

	c.JSON(http.StatusOK, metadata)
}

func (hdl MetadataHandler) GetOptions(c *gin.Context) {
	h4c := hdl.ctx.Halo4Client
	options, err := h4c.GetOptions()
	if err != nil {
		c.JSON(h4c.ErrorToHTTPStatus(err), log.Error(err.Error(), nil, nil))
		return
	}

	c.JSON(http.StatusOK, options)
}

func NewMetadataHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *MetadataHandler {
	hdl := &MetadataHandler{}
	hdl.ctx = ctx
	rg.GET("/metadata", hdl.GetMetadata)
	rg.GET("/metadata/options", hdl.GetOptions)

	return hdl
}
