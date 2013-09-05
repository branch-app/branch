class Halo4::GameHistoryController < Halo4::HomeController

	def index
		@sub_view, sub_view_was_valid = validate_param(params[:sub_view], [ 'matchmaking', 'custom-games', 'spartan-ops', 'campaign' ], 'matchmaking')
		@page, page_was_valid = validate_numerical_param(params[:page])

		redirect_to(halo4_gamehistory_path( sub_view: @sub_view || 'matchmaking', page: @page || 0 )) if !page_was_valid || !sub_view_was_valid

		@is_last_page = true
		@friendly_name = sub_view_to_friendly(@sub_view)
		@css_class = sub_view_to_css_class(@sub_view)
	end

	def view
		@css_class = 'matchmaking'
		@duration = Time.new(1994, 8, 18, 2, 45, 56, '+00:00')
	end

	private

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

end
