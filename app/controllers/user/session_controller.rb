class User::SessionController < User::HomeController
	before_filter :user_is_signed_in, only: [ :destroy ]
	before_filter :user_isnt_signed_in, only: [ :new, :create ]

	def user_is_signed_in
		redirect_to(user_signin_path) if !current_user
	end

	def user_isnt_signed_in
		redirect_to(root_path) if current_user
	end


	def new
	end

	def create
		stuff = params[:login]
		user = User.authenticate(stuff[:identifier], stuff[:password])
		if user != nil
			session[:user_id] = user.id
			redirect_to(root_path)
		else
			@error = 'Incorrect Password or Username/Email Address'
			@identifier = stuff[:identifier]
			render 'user/session/new'
		end
	end

	def destroy
		session[:user_id] = nil
		reset_session

		redirect_to(root_path)
	end
end
