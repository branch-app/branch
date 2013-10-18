class User::SessionController < User::HomeController
	before_filter :user_is_signed_in, only: [ :destroy ]
	before_filter :user_isnt_signed_in, only: [ :new, :create ]

	def user_is_signed_in
		redirect_to(user_signin_path()) if (!current_user)
	end

	def user_isnt_signed_in
		redirect_to(root_path()) if (current_user)
	end

	def new
		@return_url = params[:return_url]
	end

	def create
		stuff = params[:login]
		user = User.authenticate(stuff[:identifier], stuff[:password])
		if (user != nil)
			expire = 2.weeks.from_now()
			expire = 69.years.from_now() if (stuff[:rememberme])

			user_session = Session.new(expired: false, expires_at: expire, owner_ip: request.remote_ip, location: '', user_agent: request.env['HTTP_USER_AGENT'], user_id: user.id)
			user_session.save!

			session[:identifier] = user_session.identifier
			
			return if (redirect_to_return_url(stuff[:return_url]))
			redirect_to(user_view_path(user.username))
		else
			@error = 'Incorrect Password or Username/Email Address'
			@identifier = stuff[:identifier]
			render('user/session/new')
		end
	end

	def destroy
		user_session = Session.find_by_identifier(session[:identifier])
		if (user_session != nil)
			user_session.expired = true
			user_session.save
		end

		session[:identifier] = nil
		reset_session()

		return if (redirect_to_return_url(params[:return_url]))
		redirect_to(root_path())
	end
end
