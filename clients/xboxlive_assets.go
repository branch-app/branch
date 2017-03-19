package clients

import (
	"fmt"
	"time"

	"github.com/branch-app/service-xboxlive/models/xboxlive"
	"github.com/branch-app/shared-go/crypto"
)

const (
	colourAssetURL = "http://dlassets.xboxlive.com/public/content/ppl/colors/%s.json"
)

func (client *XboxLiveClient) GetColourAssets(colourID string) (*xboxlive.ColourAsset, error) {
	url := fmt.Sprintf(colourAssetURL, colourID)
	urlHash := crypto.CreateSHA512Hash(url)
	auth, authErr := client.GetAuthentication()

	cacheInfo := xboxlive.GetCacheInfo(client.mongoClient.DB(), xboxlive.ColourAssetsCollectionName, urlHash)
	if cacheInfo != nil && (authErr != nil || cacheInfo.CacheInformation.IsValid()) {
		return xboxlive.ColourAssetFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
	}
	if authErr != nil {
		return nil, client.handleError(nil, authErr)
	}

	// Retrieve data from Xbox Live
	var colourAssets *xboxlive.ColourAsset
	_, err := client.ExecuteRequest("GET", url, auth, 3, nil, &colourAssets)
	if err != nil {
		if cacheInfo != nil {
			return xboxlive.ColourAssetFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
		}
		return nil, client.handleError(&colourAssets.Response, err)
	}

	// Preserve ID and CreatedAt if we only updating
	if cacheInfo != nil {
		colourAssets.BranchResponse.ID = cacheInfo.ID
		colourAssets.BranchResponse.CreatedAt = cacheInfo.CreatedAt
	}

	if err := colourAssets.Upsert(client.mongoClient.DB(), url, 5*time.Minute); err != nil {
		panic(err)
	}
	return colourAssets, nil
}
