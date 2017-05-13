package clients

import (
	"fmt"
	"time"

	"github.com/branch-app/shared-go/crypto"

	"github.com/branch-app/service-halo4/models/halo4"
)

const (
	metadataURL         = "https://stats.svc.halowaypoint.com/en-US/h4/metadata"
	metadataURLWithType = "https://stats.svc.halowaypoint.com/en-US/h4/metadata?type=%s"
	optionsURL          = "https://settings.svc.halowaypoint.com/RegisterClientService.svc/register/webapp/AE5D20DCFA0347B1BCE0A5253D116752"
)

func (client *Halo4Client) GetMetadata() (*halo4.Metadata, error) {
	return client.GetMetadataWithType("")
}

func (client *Halo4Client) GetMetadataWithType(typeStr string) (*halo4.Metadata, error) {
	url := metadataURL
	if typeStr != "" {
		url = fmt.Sprintf(metadataURLWithType, typeStr)
	}

	cacheInfo := halo4.GetCacheInfo(client.mongoClient.DB(), halo4.MetadataCollectionName, crypto.CreateSHA512Hash(url))
	if cacheInfo != nil && cacheInfo.CacheInformation.IsValid() {
		return halo4.MetadataFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
	}

	// Retrieve data from Xbox Live
	var metadata *halo4.Metadata
	_, err := client.ExecuteRequest("GET", url, nil, nil, &metadata)
	if err != nil {
		if cacheInfo != nil {
			return halo4.MetadataFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
		}
		return nil, client.handleError(&metadata.Response, err)
	}

	// Preserve ID and CreatedAt if we only updating
	if cacheInfo != nil {
		metadata.BranchResponse.ID = cacheInfo.ID
		metadata.BranchResponse.CreatedAt = cacheInfo.CreatedAt
	}

	if err := metadata.Upsert(client.mongoClient.DB(), url, 720*time.Hour); err != nil {
		panic(err)
	}
	return metadata, nil
}

func (client *Halo4Client) GetOptions() (*halo4.Options, error) {
	url := optionsURL
	urlHash := crypto.CreateSHA512Hash(url)

	cacheInfo := halo4.GetCacheInfo(client.mongoClient.DB(), halo4.OptionsCollectionName, urlHash)
	if cacheInfo != nil && cacheInfo.CacheInformation.IsValid() {
		return halo4.OptionsFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
	}

	// Retrieve data from Xbox Live
	var options *halo4.Options
	_, err := client.ExecuteRequest("GET", url, nil, nil, &options)
	if err != nil {
		if cacheInfo != nil {
			return halo4.OptionsFindOne(client.mongoClient.DB(), cacheInfo.ID), nil
		}
		return nil, client.handleError(&options.Response, err)
	}

	// Preserve ID and CreatedAt if we only updating
	if cacheInfo != nil {
		options.BranchResponse.ID = cacheInfo.ID
		options.BranchResponse.CreatedAt = cacheInfo.CreatedAt
	}

	if err := options.Upsert(client.mongoClient.DB(), url, 720*time.Hour); err != nil {
		panic(err)
	}
	return options, nil
}
