class User < ActiveRecord::Base
  attr_accessible :email, :gamertag_id, :name, :password, :role_id, :username
end
