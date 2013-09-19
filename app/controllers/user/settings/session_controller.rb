class User::Settings::SessionController < User::Settings::HomeController
	def index
		@sessions = Session.where('expired = FALSE AND user_id = ? AND expires_at > CURRENT_DATE', current_user.id)
	end

	def destroy
	end
end
