package xboxlive

import (
	"fmt"
	"time"

	"github.com/branch-app/log-go"
	sharedClients "github.com/branch-app/shared-go/clients"
	sharedHelpers "github.com/branch-app/shared-go/helpers"
	"github.com/maxwellhealth/bongo"
	"gopkg.in/mgo.v2/bson"
)

type CacheRecord struct {
	bongo.DocumentBase `bson:",inline" json:"-"`
	XUID               string        `bson:"xuid"`
	DocURLHash         string        `bson:"doc_url_hash"`
	DocumentID         bson.ObjectId `bson:"document_id"`
	CachedAt           time.Time     `bson:"cached_at"`
	ExpiresAt          time.Time     `bson:"expires_at"`
	ValidDuration      time.Duration `bson:"valid_duration"`
}

const cacheRecordCollectionName = "cache_records"

func (record *CacheRecord) Save(mongo *sharedClients.MongoDBClient) error {
	return mongo.Collection(cacheRecordCollectionName).Save(record)
}

func (record *CacheRecord) Update(mongo *sharedClients.MongoDBClient) error {
	record.CachedAt = time.Now().UTC()
	record.ExpiresAt = record.CachedAt.Add(record.ValidDuration)
	return record.Save(mongo)
}

func (record *CacheRecord) IsValid() bool {
	return record.ExpiresAt.After(time.Now().UTC())
}

func CacheRecordFindOne(mongo *sharedClients.MongoDBClient, query bson.M) (*CacheRecord, error) {
	var cacheRecord *CacheRecord
	err := mongo.Collection(cacheRecordCollectionName).FindOne(query, &cacheRecord)
	if err != nil {
		fmt.Println(err)
		if _, ok := err.(*bongo.DocumentNotFoundError); ok {
			return nil, nil
		}

		branchlog.Error("mongo_find_one_error", &map[string]interface{}{
			"error": err,
		}, nil)

		return nil, err
	}

	return cacheRecord, nil
}

func NewCacheRecord(xuid, url string, docID bson.ObjectId, validDuration time.Duration) *CacheRecord {
	now := time.Now().UTC()
	return &CacheRecord{
		XUID:          xuid,
		DocURLHash:    sharedHelpers.CreateSHA512Hash(url),
		DocumentID:    docID,
		CachedAt:      now,
		ExpiresAt:     now.Add(validDuration),
		ValidDuration: validDuration,
	}
}
