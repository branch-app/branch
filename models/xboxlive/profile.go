package xboxlive

import (
	"fmt"

	sharedClients "github.com/branch-app/shared-go/clients"
	"github.com/maxwellhealth/bongo"
	"gopkg.in/mgo.v2/bson"
)

type ProfileUsers struct {
	Response `bson:",inline"`

	Users []ProfileUser `json:"profileUsers" bson:"profile_users"`
}

const profileUsersCollectionName = "profile_users"

type ProfileUser struct {
	XUID            string               `json:"id" bson:"xuid"`
	HostID          string               `json:"hostId" bson:"host_id"`
	Settings        []ProfileUserSetting `json:"settings" bson:"settings"`
	IsSponsoredUser bool                 `json:"isSponsoredUser" bson:"is_sponsored_user"`
}

func (profileUsers *ProfileUsers) Save(mongo *sharedClients.MongoDBClient) error {
	return mongo.Collection(profileUsersCollectionName).Save(profileUsers)
}

func ProfileUsersFindOne(mongo *sharedClients.MongoDBClient, query bson.M) (*ProfileUsers, error) {
	var profileUsers *ProfileUsers
	err := mongo.Collection(profileUsersCollectionName).FindOne(query, &profileUsers)
	if err != nil {
		fmt.Println(err)
		if _, ok := err.(*bongo.DocumentNotFoundError); ok {
			return nil, nil
		}

		return nil, err
	}

	return profileUsers, nil
}

type ProfileUserSetting struct {
	ID    string `json:"id" bson:"id"`
	Value string `json:"value" bson:"value"`
}
