class Halo4::ServiceRecordController < Halo4::HomeController
	def service_record
		@title = "Branch - #{@identity['gamertag']}'s Halo 4 Service Record"
	end
end
