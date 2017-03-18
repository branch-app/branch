class Halo4::ServiceRecordController < Halo4::HomeController
	def index
		@title = "Branch - #{@identity['gamertag']}'s Halo 4 Service Record"
		@sidebar = { service_record: 'active' }
	end
end
