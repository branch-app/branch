class Session < ActiveRecord::Base
	attr_accessible :expired, :expires_at, :identifier, :owner_ip, :user_id, :location, :gps_loc, :user_agent, :platform, :browser, :version

	belongs_to :user

	before_create :generate_identifier
	before_create :generate_useragent_data
	before_create :refine_ip_location

	private
		def generate_identifier
			self.identifier = SecureRandom.uuid()
		end
		def generate_useragent_data
			agent = Agent.new(user_agent)
			self.platform = agent.os
			self.browser = agent.name
			self.version = agent.version
		end
		def refine_ip_location
			geocode = Geokit::Geocoders::MultiGeocoder.geocode(owner_ip)
			puts geocode.to_hash
			return if geocode == nil && geocode.Success
			geocode = geocode.to_hash
			self.location = geocode[:full_address]
			self.gps_loc = geocode[:ll]
		end
end
