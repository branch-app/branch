class Halo4::HomeController < ApplicationController
	before_filter :get_gamertag

	@gamertag = nil
	def get_gamertag
		@gamertag = params[:gamertag]

		# pull stats
		@service_record = H4Api.get_player_service_record(@gamertag)

		if (@service_record == nil || @service_record[:continue] == false || @service_record["StatusCode"] != 1)
			flash[:failure] = "We couldn't load stats for the gamertag #{@gamertag}, sorry :("
			redirect_to(root_url())
			return
		end
	end

	def index
		
	end

end
