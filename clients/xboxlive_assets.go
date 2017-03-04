package clients

import (
	"time"

	"fmt"

	"github.com/branch-app/shared-go/models/branch"
	"github.com/branch-app/service-xboxlive/models/xboxlive"
	"github.com/branch-app/shared-go/crypto"
	sharedModels "github.com/branch-app/shared-go/models"
	"gopkg.in/mgo.v2/bson"
)

const (
	colourAssetURL = "http://dlassets.xboxlive.com/public/content/ppl/colors/%s.json"
)

func (client *XboxLiveClient) GetColourAssets(colourID string) (*branch.Response, error) {
	url := fmt.Sprintf(colourAssetURL, colourID)
	urlHash := crypto.CreateSHA512Hash(url)

	// Check if we have a cached document
	cacheRecord, err := sharedModels.CacheRecordFindOne(client.mongoClient, bson.M{"doc_url_hash": urlHash})
	fmt.Println(cacheRecord)
	if err != nil {
		return nil, err
	}
	if cacheRecord != nil && cacheRecord.IsValid() {
		response, err := xboxlive.ColourAssetFindOne(client.mongoClient, bson.M{"_id": cacheRecord.DocumentID})
		if err != nil {
			return nil, err
		}
		return branch.NewResponse(cacheRecord.CachedAt, response), nil
	}

	// Retrieve data from Xbox Live
	var colourAsset *xboxlive.ColourAsset
	_, err = client.ExecuteRequest("GET", url, nil, 3, nil, &colourAsset)
	if err != nil {
		if err != nil {
			colourAsset, _ = xboxlive.ColourAssetFindOne(client.mongoClient, bson.M{"_id": cacheRecord.DocumentID})
			if colourAsset != nil {
				return branch.NewResponse(cacheRecord.CachedAt, &colourAsset), nil
			}
			return nil, client.handleError(&colourAsset.Response, err)
		}
	}

	// Deal with caching
	if cacheRecord != nil {
		// Already have a cache record - so just update
		colourAsset.Id = cacheRecord.DocumentID
		colourAsset.Save(client.mongoClient)

		// Update Cache Record
		cacheRecord.Update(client.mongoClient)
	} else {
		// Need to add to database
		colourAsset.Save(client.mongoClient)
		cacheRecord = sharedModels.NewCacheRecord(url, colourAsset.Id, 1460*time.Hour) // 2 months
		cacheRecord.Save(client.mongoClient)
	}

	return branch.NewResponse(cacheRecord.CachedAt, colourAsset), nil
}
