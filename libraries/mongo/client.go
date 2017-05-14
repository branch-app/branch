package mongo

import (
	"github.com/branch-app/branch-mono-go/domain/branch"
	"github.com/branch-app/branch-mono-go/libraries/log"
	mgo "gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

type Client struct {
	session  *mgo.Session
	database *mgo.Database
}

// GetDocumentCacheInformation returns the branch specific cache information of the
// document
func (client *Client) GetDocumentCacheInformation(collection, hash string) (*branch.Response, *log.E) {
	query := client.database.
		C(collection).
		Find(bson.M{"cacheInformation.docUrlHash": hash}).
		Select(bson.M{"cacheInformation": 1, "_id": 1, "createdAt": 1, "updatedAt": 1})

	var cacheInfo *branch.Response
	err := coerceMongoError(query.One(&cacheInfo))
	if err != nil {
		return nil, err
	}

	return cacheInfo, nil
}

// FindByID try's to get a document from a specified collection by it's ID.
func (client *Client) FindByID(id bson.ObjectId, collection string, response interface{}) *log.E {
	query := client.database.
		C(collection).
		FindId(id)

	err := query.One(response)
	return coerceMongoError(err)
}

// FindByCacheInfoHash try's to get a document from a specified collection by it's cache
// document url hash.
func (client *Client) FindByCacheInfoHash(hash string, collection string, response interface{}) *log.E {
	query := client.database.
		C(collection).
		Find(createCacheInfoSelector(hash))

	return coerceMongoError(query.One(&response))
}

// Close will close the Mongo DB session active in this client.
func (client *Client) Close() {
	client.session.Close()
}

// Upsert inserts or updates a document by it's selector.
func (client *Client) Upsert(selector, data interface{}, collection string) *log.E {
	// If id is invalid, set it now

	_, err := client.database.
		C(collection).
		Upsert(selector, data)

	return coerceMongoError(err)
}

// UpsertByCacheInfoHash inserts or updates a document by it's selector.
func (client *Client) UpsertByCacheInfoHash(hash string, data interface{}, collection string) *log.E {
	return client.Upsert(createCacheInfoSelector(hash), data, collection)
}

// createCacheInfoSelector creates a selector that will look for an item by the
// docUrlHash inside it's cacheinfo.
func createCacheInfoSelector(hash string) interface{} {
	return bson.M{"cacheInformation.docUrlHash": hash}
}

func coerceMongoError(err error) *log.E {
	if err == nil {
		return nil
	}

	switch err.Error() {
	case "not found":
		return log.Info("document_not_found", nil)
	}

	panic(err)
}

func NewClient(connectionString string, dbName string, log bool) *Client {
	session, err := mgo.Dial(connectionString)
	if err != nil {
		panic(err)
	}

	session.SetMode(mgo.Monotonic, true)
	database := session.DB(dbName)

	return &Client{
		session:  session,
		database: database,
	}
}
