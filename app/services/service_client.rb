require('singleton')

class ServiceClient
	include Singleton

	def initialize()
		@serviceInfo = {
			'service-auth'     => 'http://localhost:3000/v1',
			'service-xboxlive' => 'http://localhost:3010/v1',
			'service-halo4'    => 'http://localhost:3020/v1',
			'service-branch'   => 'http://localhost:3030/v1',
		}
	end

	def get(serviceId, path)
		return execute_request(:get, serviceId, path, nil)
	end

	def execute_request(method, serviceId, path, body)
		serviceUrl = @serviceInfo[serviceId]
		request = Typhoeus::Request.new(
			serviceUrl + path,
			method: method,
			headers: { ContentType: 'application/json' },
			followlocation: true,
		)

		request.on_complete do |response|
			if response.success?
				return JSON.parse(response.body)
			elsif response.headers['content-type'].include? 'application/json'
				raise BranchError.coerce(response.body)
			else
				raise BranchError.new('unknown_error', response.code)
			end
		end

		request.run
	end
end
