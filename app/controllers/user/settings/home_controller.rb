class User::Settings::HomeController < User::HomeController
	before_filter :validate_user

	def validate_user
		redirect_to(siguser_signin_path) if !current_user
	end
end
