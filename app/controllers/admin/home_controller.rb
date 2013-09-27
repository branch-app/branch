class Admin::HomeController < ApplicationController
	layout('admin')
	before_filter :validate_admin

	def validate_admin
		user = current_user()
		redirect_to(root_url) if user == nil || user.role.identifier != 3
	end

	def index
		
	end
end
