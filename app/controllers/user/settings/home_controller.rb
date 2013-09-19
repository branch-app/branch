class User::Settings::HomeController < User::HomeController
	before_filter :validate_user

	def validate_user
		redirect_to(user_signin_path) if !current_user
	end
end
