class UserVerification < ActiveRecord::Base
  attr_accessible :has_verified, :identifier, :user_id
end
