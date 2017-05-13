package handlers

import (
	"net/http"

	"fmt"

	log "github.com/branch-app/log-go"
	"github.com/branch-app/service-halo4/contexts"
	sharedHelpers "github.com/branch-app/shared-go/helpers"
	sharedModels "github.com/branch-app/shared-go/models"

	"gopkg.in/gin-gonic/gin.v1"
)

type ServiceRecordHandler struct {
	ctx *contexts.ServiceContext
}

const identityLookupURL = "/identity/%s(%s)"

func (hdl ServiceRecordHandler) Get(c *gin.Context) {
	identityCall := sharedHelpers.ParseIdentity(c.Param("identity"))
	if identityCall == nil {
		c.JSON(http.StatusBadRequest, log.Error("invalid_identity", nil, nil))
		return
	}

	svc := hdl.ctx.ServiceClient
	var identity *sharedModels.XboxLiveIdentity
	_, err := svc.Get("service-xboxlive", fmt.Sprintf(identityLookupURL, identityCall.Type, identityCall.Identity), &identity)
	if err != nil {
		c.JSON(http.StatusNotFound, log.Error(err.Error(), nil, nil))
		return
	}

	h4c := hdl.ctx.Halo4Client
	serviceRecord, err := h4c.GetServiceRecord(identity)
	if err != nil {
		c.JSON(h4c.ErrorToHTTPStatus(err), log.Error(err.Error(), nil, nil))
		return
	}
	c.JSON(http.StatusOK, serviceRecord)
}

func NewServiceRecordHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *ServiceRecordHandler {
	hdl := &ServiceRecordHandler{}
	hdl.ctx = ctx
	rg.GET("/identity/:identity/service-record", hdl.Get)

	return hdl
}
