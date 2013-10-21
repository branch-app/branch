class ApplicationController < ActionController::Base
	include ApplicationHelper
	helper_method :current_user
	helper_method :set_flash_message
	helper_method :redirect_to_return_url
	before_filter :set_flash
	protect_from_forgery

	def set_flash()
		if (params[:force_flash].to_i == 1)
			set_flash_message('success', 'Woo!', 'This is a success message!')
			set_flash_message('failure', 'Fuck...', 'Something bad happened, and im here to tell you that.')
			set_flash_message('warning', 'ehh..', 'Something bad might happen, so heads up.')
			set_flash_message('info', 'hey m8!', "Did you know this is a information message? I know right, it's pretty cool")
		end
	end

	# -- Internal Functions
	def sub_view_to_friendly(sub_view)
		return sub_view.gsub('-', ' ').titlecase()
	end

	def sub_view_to_css_class(sub_view)
		return sub_view.gsub('-', '_')
	end

	def validate_param(param, valid_responses = nil, default_response = nil)
		return default_response || nil, false if (param == nil || param.strip == '')
		return param, true if (valid_responses == nil || default_response == nil)

		# check valid responses
		valid_responses.each { |r| return param, true if (param.strip.downcase == r) }
		return default_response, false
	end

	def validate_numerical_param(param, min = 0, max = 0xFFFFFFFF)
		param = param.to_i
		return 0, false if (param == nil)

		if (param >= min && param <= max)
			return param, true
		else
			return 0, false
		end
	end

	# -- Helpers
	def current_user
		begin
			user_session = Session.find_by_identifier(session[:identifier]) if (session[:identifier])
			raise if (user_session == nil || user_session.expired || user_session.expires_at < DateTime.now)
			@current_user = User.find_by_id(user_session.user_id)
		rescue
			reset_session
			@current_user = nil
		end
	end

	def set_flash_message(type, title, desc)
		flash[type.to_sym] = { title: title, desc: desc }
	end

	def redirect_to_return_url(return_url)
		if (return_url)
			begin
				uri = URI.parse(return_url)
				if (uri.host == 'localhost' || uri.host == 'branchapp')
					redirect_to(return_url)
					return true
				end
			rescue
				return false
			end
		end

		return false
	end
end
