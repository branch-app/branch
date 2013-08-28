class Halo4::HomeController < ApplicationController
	before_filter :get_gamertag

	@gamertag = nil
	def get_gamertag
		@gamertag = params[:gamertag]
	end

	def index
		
	end

end
