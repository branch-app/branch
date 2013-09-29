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
			session[:identifier] = user_session.identifier
			redirect_to(user_view_path(@user.username))
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

	def resend_verification
		if !current_user
			redirect_to(user_signin_path) 
			return
		end
		if current_user.role_id != Role.find_by_identifier(1).id
			flash[:failure] = "You can't resend a verification email to an verified account"
			redirect_to(root_path)
			return
		end

		current_user.set_to_validating()
		flash[:failure] = "Verification email resent"
		redirect_to(user_view_path(id: current_user.username))
	end
end
