package routing

import (
	"net/http"

	"github.com/branch-app/branch-mono-go/libraries/log"

	"github.com/golang/gddo/httputil"
)

var (
	offers = []string{
		"application/json",
	}

	errors = map[string]int{
		"incompatible_accept_header": http.StatusNotAcceptable,
	}
)

func ParseOutput(c *Context, status int, data interface{}) *log.E {
	offer := httputil.NegotiateContentType(c.GinContext.Request, offers, "")
	if offer == "" {
		return log.Info("incompatible_accept_header", nil)
	}

	switch offer {
	default:
		return log.Info("incompatible_accept_header", nil)

	case "application/json":
		c.GinContext.JSON(status, &data)
	}

	return nil
}

func PresentError(c *Context, err *log.E) {
	status := http.StatusInternalServerError
	for c, s := range errors {
		if c == err.Code {
			status = s
		}
	}

	c.GinContext.JSON(status, &err)
}
