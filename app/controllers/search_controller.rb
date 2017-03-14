class SearchController < ApplicationController
	def index
		playerIdent = params['playerIdent']
		redirect_to(controller: 'xbox_live/profile', action: 'profile', gamertag: playerIdent)
	end
end
