class SearchController < ApplicationController
	def index
		@halo4Gamertag = X343ApiController.GetServiceRecord(params[:gamertag])
		@halo4PlayerModel = X343ApiController.GetPlayerModel(params[:gamertag], 'medium')
	end
end