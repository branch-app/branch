class User::Account::FollowingController < User::Account::HomeController
	def index
		@following = @account.following()
	end
end
