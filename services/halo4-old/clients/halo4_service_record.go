package clients

import (
	"fmt"
	"time"

	"github.com/branch-app/shared-go/crypto"
	sharedModels "github.com/branch-app/shared-go/models"

	"github.com/branch-app/service-halo4/models/halo4"
)

const (
	serviceRecordURL = "https://stats.svc.halowaypoint.com/en-US/players/%s/h4/servicerecord"
)

func (client *Halo4Client) GetServiceRecord(identity *sharedModels.XboxLiveIdentity) (*halo4.ServiceRecord, error) {
	url := fmt.Sprintf(serviceRecordURL, identity.Gamertag)
	urlHash := crypto.CreateSHA512Hash(url)
	auth, authErr := client.GetAuthentication()

	cacheInfo := halo4.GetCacheInfo(client.mongoClient.DB(), halo4.ServiceRecordCollectionName, urlHash)
	if cacheInfo != nil && (authErr != nil || cacheInfo.CacheInformation.IsValid()) {
		return halo4.ServiceRecordFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
	}
	if authErr != nil {
		return nil, client.handleError(nil, authErr)
	}

	// Retrieve data from Xbox Live
	var serviceRecord *halo4.ServiceRecord
	_, err := client.ExecuteRequest("GET", url, auth, nil, &serviceRecord)
	if err != nil {
		if cacheInfo != nil {
			return halo4.ServiceRecordFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
		}
		return nil, client.handleError(&serviceRecord.Response, err)
	}

	// Preserve ID and CreatedAt if we only updating
	if cacheInfo != nil {
		serviceRecord.BranchResponse.ID = cacheInfo.ID
		serviceRecord.BranchResponse.CreatedAt = cacheInfo.CreatedAt
	}

	if err := serviceRecord.Upsert(client.mongoClient.DB(), url, 5*time.Minute); err != nil {
		panic(err)
	}
	return serviceRecord, nil
}
