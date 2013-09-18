class User::UserController < User::HomeController
	before_filter :validate_user, only: [ :new, :create ]

	def validate_user
		
	end

	def index
		
	end

	def new
		@user = User.new
	end

	def create
		@user = User.new(params[:user])
		if @user.save
			session[:user_id] = @user.id
			redirect_to(root_path)
		else
			render 'user/user/new'
		end
	end
end
