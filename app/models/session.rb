class Session < ActiveRecord::Base
	attr_accessible :expired, :expires_at, :identifier, :owner_ip, :user_id, :location, :user_agent

	belongs_to :user

	before_create :generate_identifier

	private
		def generate_identifier
			self.identifier = rand(0..100000).to_s
		end
end
