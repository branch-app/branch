class Admin::HomeController < ApplicationController
	layout('admin')
	before_filter :validate_admin
	before_filter :generate_colour

	def validate_admin
		user = current_user()
		redirect_to(root_url) if user == nil || user.role.identifier != 3
	end

	@watermark = 'fff'
	def generate_colour
		username = current_user().username
		seed = 0
		username.each_byte do |c|
			seed += c.to_i
		end
		@watermark = (((Math.sin(seed) * 16777215).abs() % 16777215).floor()).to_s(16)
	end

	def index
		
	end
end
