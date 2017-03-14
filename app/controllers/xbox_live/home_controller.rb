class XboxLive::HomeController < ApplicationController
	before_action :require_gamertag, except: :index

	def index
		
	end

	private

	def require_gamertag
		gamertag = CGI.escape(params[:gamertag])

		begin
			identity = ServiceClient.instance.get('service-xboxlive', "/identity/gamertag(#{gamertag})/")
			puts identity.inspect

		rescue BranchError => e
			if e.code == "invalid_identity"
				flash[:warning] = "The provided Gamertag doesn't exist."
			end

			redirect_to(controller: 'xbox_live/home', action: 'index')
		end
	end
end
