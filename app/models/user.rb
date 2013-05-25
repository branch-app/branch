require 'pbkdf2'

class User < ActiveRecord::Base
	#attr_accessible :email, :gamertag, :is_admin, :name, :password_crypted, :username

	def self.authenticate(signin_id, password)
		user = find_by_email(signin_id)
		unless user
			user = find_by_username(signin_id)
		end

		if user && Hashing.validate(password, user.password_hash)
			user
		else
			nil
		end
	end
end
