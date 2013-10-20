class User::UserController < User::HomeController
	before_filter :validate_user_unauthed, only: [ :new, :create ]
	before_filter :validate_user_authed, only: [ :follow ]

	def validate_user_unauthed
		redirect_to(root_path()) if (current_user)
	end

	def validate_user_authed
		redirect_to(root_path()) if (!current_user)
	end

	def new
		@user = User.new()
	end

	def create
		@user = User.new(params[:user])
		if (@user.save)
			user_session = Session.new(expired: false, expires_at: 2.weeks.from_now, owner_ip: request.remote_ip, location: '', user_agent: request.env['HTTP_USER_AGENT'], user_id: @user.id)
			user_session.save!()
			session[:identifier] = user_session.identifier
			redirect_to(user_view_path(@user.username))
		else
			render 'user/user/new'
		end
	end

	def verify
		verification_id = params[:verification_id]
		verification = UserVerification.verify(verification_id)

		if (verification === true)
			flash[:success] = 'You have successfully verified your account. Thanks!'
			redirect_to(root_path)
			return
		end
	end

	def resend_verification
		if (!current_user)
			redirect_to(user_signin_path()) 
			return
		end
		if (current_user.role_id != Role.find_by_identifier(1).id)
			flash[:failure] = "You can't resend a verification email to an verified account"
			redirect_to(root_path())
			return
		end

		current_user.set_to_validating()
		flash[:failure] = "Verification email resent"
		redirect_to(user_view_path(id: current_user.username))
	end

	def follow
		user_a = User.find_by_id(params[:follow][:user_in_question].to_i)
		user_b = current_user
		
		if (user_a == nil || user_b == nil)
			render json: { state: nil, success: false, error: { name: 'invalid_user_shit', desc: "Somewhere along the way a user id broke (not good...). Check you're logged in, and that the user you're following actually exists..." } }
			return
		end

		if (user_b.following?(user_a))
			# delete follow
			follow = Follow.find_by_follower_id_and_following_id(user_b.id, user_a.id)

			if (follow == nil)
				render json: { state: nil, success: false, error: { name: 'invalid_user_shit', desc: "Somewhere along the way a user id broke (not good...). Check you're logged in, and that the user you're following actually exists..." } }
				return
			end

			if (follow.destroy())
				render json: { state: 'follow', success: true, response: { happy_message: "You're not following #{user_a.username} anymore :(." } }
				return
			else
				render json: { state: nil, success: false, error: { name: 'model_fucked_up_big_time', desc: "Somewhere along the way a user id broke (not good...). Check you're logged in, and that the user you're following actually exists..." } }
				return
			end
		else
			follow = Follow.new(follower_id: user_b.id, following_id: user_a.id)

			if (follow.valid?() && follow.save())
				render json: { state: 'following', success: true, response: { happy_message: "You're now following #{user_a.username}. Good on you man :)" } }
				return
			else
				render json: { state: nil, success: false, error: { name: 'model_fucked_up_big_time', desc: follow.errors[:base] } }
				return
			end
		end
	end
end
