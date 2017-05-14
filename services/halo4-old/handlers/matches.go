package handlers

import (
	"net/http"
	"strconv"

	"fmt"

	log "github.com/branch-app/log-go"
	"github.com/branch-app/service-halo4/contexts"
	sharedHelpers "github.com/branch-app/shared-go/helpers"
	sharedModels "github.com/branch-app/shared-go/models"

	"gopkg.in/gin-gonic/gin.v1"
)

type MatchesHandler struct {
	ctx *contexts.ServiceContext
}

func (hdl MatchesHandler) GetRecentMatches(c *gin.Context) {
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

	modeID, err := strconv.Atoi(c.Query("modeId"))
	if err != nil {
		c.JSON(http.StatusBadRequest, log.NewBranchError("invalid_mode_id", nil, nil))
		return
	}
	startAt, err := strconv.Atoi(c.DefaultQuery("startAt", "0"))
	if err != nil {
		c.JSON(http.StatusBadRequest, log.NewBranchError("invalid_start_at", nil, nil))
		return
	}
	count, err := strconv.Atoi(c.DefaultQuery("count", "25"))
	if err != nil {
		c.JSON(http.StatusBadRequest, log.NewBranchError("invalid_count", nil, nil))
		return
	}

	h4c := hdl.ctx.Halo4Client
	recentMatches, err := h4c.GetRecentMatches(identity, modeID, startAt, count)
	if err != nil {
		c.JSON(h4c.ErrorToHTTPStatus(err), log.Error(err.Error(), nil, nil))
		return
	}
	c.JSON(http.StatusOK, recentMatches)
}

func (hdl MatchesHandler) GetMatch(c *gin.Context) {
	matchID := c.Param("match_id")
	h4c := hdl.ctx.Halo4Client
	match, err := h4c.GetMatch(matchID)
	if err != nil {
		c.JSON(h4c.ErrorToHTTPStatus(err), log.Error(err.Error(), nil, nil))
		return
	}
	c.JSON(http.StatusOK, match)
}

func NewMatchesHandler(rg *gin.RouterGroup, ctx *contexts.ServiceContext) *MatchesHandler {
	hdl := &MatchesHandler{}
	hdl.ctx = ctx
	rg.GET("/identity/:identity/matches", hdl.GetRecentMatches)
	rg.GET("/matches/:match_id", hdl.GetMatch)

	return hdl
}
