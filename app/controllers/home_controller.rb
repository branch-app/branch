class HomeController < ApplicationController

	def index
		redirect_to(root_path()) if (!current_user)
	end

	def welcome
		redirect_to(home_dashboard_path()) if (current_user)
	end

end
