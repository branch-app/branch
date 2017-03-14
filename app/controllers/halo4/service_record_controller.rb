class Halo4::ServiceRecordController < Halo4::HomeController
	def service_record
		begin
			@service_record = ServiceClient.instance.get('service-halo4', "/identity/gamertag(#{@gamertag})/service-record")
			@recent_matches = ServiceClient.instance.get('service-halo4', "/identity/gamertag(#{@gamertag})/recent-matches?modeId=3&count=5")

		rescue BranchError => e
			raise
		end
	end
end
