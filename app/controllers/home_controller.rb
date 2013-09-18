class HomeController < ApplicationController

	def index
		redirect_to(:home_welcome) unless current_user
	end

	def welcome
		
	end

end
