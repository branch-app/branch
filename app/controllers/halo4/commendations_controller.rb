class Halo4::CommendationsController < Halo4::HomeController

	def index
		@sub_view, sub_view_was_valid = validate_param(params[:sub_view], [ 'weapon', 'enemy', 'vehicle', 'player', 'game-type' ], 'weapon')

		redirect_to(halo4_commendations_path( sub_view: @sub_view || 'weapon')) if !sub_view_was_valid

		@friendly_name = sub_view_to_friendly(@sub_view)
		@css_class = sub_view_to_css_class(@sub_view)
	end

end
