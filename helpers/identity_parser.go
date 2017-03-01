package helpers

import (
	"fmt"
	"regexp"
	"strings"

	"github.com/branch-app/log-go"
	"github.com/branch-app/service-xboxlive/models"
)

func ParseIdentity(identityParam string) (*models.IdentityCall, error) {
	identityParam = strings.ToLower(identityParam)
	identities := []string{"xuid", "gamertag"}
	r := regexp.MustCompile(fmt.Sprintf(`(?i)(?P<Type>%s)\((?P<Identifier>[a-z0-9 ]+)\)`, strings.Join(identities, "|")))
	matched := r.FindStringSubmatch(identityParam)
	if len(matched) < 3 {
		bErr := branchlog.Error("invalid_identity", nil, &map[string]interface{}{
			"identityParam": identityParam,
		})

		return nil, bErr.Error()
	}

	identityCall := &models.IdentityCall{
		Type:     matched[1],
		Identity: matched[2],
	}
	return identityCall, nil
}
