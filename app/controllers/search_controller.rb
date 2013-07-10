class SearchController < ApplicationController
	include ApplicationHelper
	
	def index
		@halo4Gamertag = I343ApiH4.get_player_service_record(params[:q])
		
		if @halo4Gamertag[:continue] == 'no'
			setup_error_notification('error', "The gamertag `#{params[:q]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end

		@halo4PlayerModel = I343ApiH4.get_player_model(params[:q], 'medium')
	end
end