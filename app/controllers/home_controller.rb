class HomeController < ApplicationController

	def index
		redirect_to(home_welcome_path()) if (!current_user)
	end

	def welcome
		
	end

end
