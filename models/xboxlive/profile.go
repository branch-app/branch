package xboxlive

import (
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"

	"time"

	sharedModels "github.com/branch-app/shared-go/models"
)

type ProfileUsers struct {
	Response `bson:",inline"`

	Users []ProfileUser `json:"profileUsers" bson:"profileUsers"`
}

const ProfileUsersCollectionName = "profile_users"

func (profUsers *ProfileUsers) Upsert(db *mgo.Database, url string, validFor time.Duration) error {
	now := time.Now().UTC()

	// Check if we need to set CreatedAt and ID
	if !profUsers.BranchResponse.ID.Valid() {
		profUsers.BranchResponse.ID = bson.NewObjectId()
		profUsers.BranchResponse.CreatedAt = now
	}

	profUsers.BranchResponse.UpdatedAt = now
	profUsers.BranchResponse.CacheInformation = sharedModels.NewCacheInformation(url, now, validFor)
	_, err := db.C(ProfileUsersCollectionName).UpsertId(profUsers.BranchResponse.ID, profUsers)
	return err
}

func ProfileUsersFindOne(db *mgo.Database, id bson.ObjectId) *ProfileUsers {
	var document *ProfileUsers
	err := db.C(ProfileUsersCollectionName).FindId(id).One(&document)
	switch {
	case err == nil:
		return document
	case err.Error() == "not found":
		return nil
	default:
		panic(err)
	}
}

type ProfileUser struct {
	XUID            string               `json:"id" bson:"xuid"`
	HostID          string               `json:"hostId" bson:"hostId"`
	Settings        []ProfileUserSetting `json:"settings" bson:"settings"`
	IsSponsoredUser bool                 `json:"isSponsoredUser" bson:"isSponsoredUser"`
}

type ProfileUserSetting struct {
	ID    string `json:"id" bson:"id"`
	Value string `json:"value" bson:"value"`
}
