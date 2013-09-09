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
	
end
