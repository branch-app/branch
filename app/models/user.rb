class User < ActiveRecord::Base
	attr_accessible :email, :gamertag_id, :gamertag, :name, :password, :password_confirmation, :role_id, :username
	attr_accessor :password_confirmation, :gamertag

	validate_presence_of :email, :gamertag, :name

	validates_length_of :username, in: 1..30
	validates_length_of :name, in: 3..30

	validates_uniqueness_of :email
	validates_uniqueness_of :username

	
end
