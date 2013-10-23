class User::Account::HomeController < User::HomeController
	include Halo4::HomeHelper
	before_filter :get_account
	before_filter :get_h4_metadata
	def get_account
		@account = User.find_by_username(params[:id])
	end

	def get_h4_metadata
		@metadata = H4Api.get_meta_data()
	end

	def index
		
	end
end
