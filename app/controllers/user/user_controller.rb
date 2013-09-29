class User::UserController < User::HomeController
	before_filter :validate_user_unauthed, only: [ :new, :create ]

	def validate_user_unauthed
		redirect_to(root_path) if current_user
	end

	def new
		@user = User.new
	end

	def create
		@user = User.new(params[:user])
		if @user.save
			user_session = Session.new(expired: false, expires_at: 2.weeks.from_now, owner_ip: request.remote_ip, location: '', user_agent: request.env['HTTP_USER_AGENT'], user_id: @user.id)
			user_session.save!
			session[:session_id] = user_session.identifier
			redirect_to(root_path)
		else
			render 'user/user/new'
		end
	end

	def verify
		verification_id = params[:verification_id]
		verification = UserVerification.verify(verification_id)

		if verification === true
			flash[:success] = 'You have successfully verified your account. Thanks!'
			redirect_to(root_path)
			return
		end
	end
end
