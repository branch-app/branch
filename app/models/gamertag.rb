class Gamertag < ActiveRecord::Base
  attr_accessible :gamertag

  has_many :users
end
