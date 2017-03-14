require('singleton')

class Halo4Client
	include Singleton

	def initialize()
		@service_id = "service-halo4"

		begin
			@metadata = ServiceClient.instance.get(@service_id, '/metadata')
			@metadata_options = ServiceClient.instance.get(@service_id, '/metadata/options')
		rescue BranchError => e
			raise
		end
	end
end
