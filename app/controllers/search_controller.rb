class SearchController < ApplicationController
	include ApplicationHelper
	
	def index
		@halo4Gamertag = X343ApiController.GetServiceRecord(params[:q])

		if @halo4Gamertag[:continue] == 'no'
			setup_error_notification('error', "The gamertag `#{params[:q]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end

		@halo4PlayerModel = X343ApiController.GetPlayerModel(params[:q], 'medium')
	end
end