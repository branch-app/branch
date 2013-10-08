class User::Settings::SessionController < User::Settings::HomeController
	def index
		@sessions = Session.where('expired = FALSE AND user_id = ? AND expires_at > CURRENT_DATE', current_user.id).order('created_at DESC')
	end

	def destroy
		session_id = params[:session][:sid]
		user_session = Session.find_by_id(session_id)

		if (user_session == nil)
			flash[:info] = 'Unable to murder a non-existant session'
			render('user/settings/session')
			return
		end

		if (user_session.user_id != current_user.id)
			flash[:danger] = "Don't even try to murder someone else's session, you fucking wanker"
			render('user/settings/session')
			return
		end

		user_session.expired = true
		user_session.save()

		redirect_to(settings_session_path())
	end
end
