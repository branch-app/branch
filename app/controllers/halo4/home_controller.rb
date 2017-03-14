require('addressable/uri')

class Halo4::HomeController < ApplicationController
	before_action :require_gamertag, except: :index

	def index
		
	end

	private

	def require_gamertag
		@gamertag = Addressable::URI.escape(params[:gamertag])

		begin
			@identity = ServiceClient.instance.get('service-xboxlive', "/identity/gamertag(#{@gamertag})/")
			puts @identity.inspect

		rescue BranchError => e
			if e.code == "invalid_identity"
				flash[:warning] = "The provided Gamertag doesn't exist."
			end

			redirect_to(controller: 'halo4/home', action: 'index')
		end
	end
end
