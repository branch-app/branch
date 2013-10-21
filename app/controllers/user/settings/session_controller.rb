class User::Settings::SessionController < User::Settings::HomeController
	def index
		@sessions = Session.where('expired = FALSE AND user_id = ? AND expires_at > CURRENT_DATE', current_user.id).order('created_at DESC')
	end

	def destroy
		session_id = params[:session][:sid]
		user_session = Session.find_by_id(session_id)

		if (user_session == nil)
			set_flash_message('info', 'Hey, Btw', "You can't murder a session that doesn't exist...")
			render('user/settings/session')
			return
		end

		if (user_session.user_id != current_user.id)
			set_flash_message('failure', 'What the fuck...', "Don't even try to murder someone else's session...")
			render('user/settings/session')
			return
		end

		user_session.expired = true
		user_session.save()

		redirect_to(settings_session_path())
	end
end
