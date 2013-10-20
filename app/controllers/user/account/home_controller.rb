class User::Account::HomeController < User::HomeController
	include Halo4::HomeHelper
	before_filter :get_account
	def get_account
		@account = User.find_by_username(params[:id])
	end

	def index
		
	end
end
