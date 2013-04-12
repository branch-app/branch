class AuthenticationVault < ActiveRecord::Base
  attr_accessible :spartan_token, :wlid_access_token, :wlid_authentication_token, :wlid_expire
end
