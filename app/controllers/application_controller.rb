class ApplicationController < ActionController::Base
	helper_method :current_user
	protect_from_forgery

	# -- Internal Functions
	def sub_view_to_friendly(sub_view)
		return sub_view.gsub('-', ' ').titlecase
	end

	def sub_view_to_css_class(sub_view)
		return sub_view.gsub('-', '_')
	end

	def validate_param(param, valid_responses = nil, default_response = nil)
		return default_response || nil, false if param == nil || param.strip == ''
		return param, true if valid_responses == nil || default_response == nil

		# check valid responses
		valid_responses.each { |r| return param, true if param.strip.downcase == r }
		return default_response, false
	end

	def validate_numerical_param(param, min = 0, max = 0xFFFFFFFF)
		param = param.to_i
		return 0, false if param == nil

		if param >= min && param <= max
			return param, true
		else
			return 0, false
		end
	end

	# -- Helpers
	def current_user
		begin
			user_session = Session.find_by_identifier(session[:identifier]) if session[:identifier]
			raise if user_session == nil || user_session.expired || user_session.expires_at < DateTime.now
			@current_user = User.find_by_id(user_session.user_id)
		rescue
			reset_session
			@current_user = nil
		end
	end

end
