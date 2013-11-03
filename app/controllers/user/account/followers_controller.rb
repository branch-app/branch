class User::Account::FollowersController < User::Account::HomeController
	def index
		@followers = @account.followers()
	end
end
