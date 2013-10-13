class Gamertag < ActiveRecord::Base
  attr_accessible :gamertag

  has_many :users
  has_many :h4_service_record
end
