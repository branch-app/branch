class User::Account::HomeController < User::HomeController
	before_filter :validate_username

	def validate_username
		@username = params[:username]
	end

	def index
	end
end
