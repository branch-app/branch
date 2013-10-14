class Halo4::CommendationsController < Halo4::HomeController

	def index
		@sub_view, sub_view_was_valid = validate_param(params[:sub_view], [ 'weapon', 'enemy', 'vehicle', 'player', 'game-type' ], 'weapon')

		if (!sub_view_was_valid)
			redirect_to(halo4_commendations_path( sub_view: @sub_view || 'weapon'))
			return
		end

		@friendly_name = sub_view_to_friendly(@sub_view)
		@css_class = sub_view_to_css_class(@sub_view)
		@category_id = 0

		case(@sub_view)
			when 'weapon'
				@category_id = 1
			when 'enemy'
				@category_id = 3
			when 'vehicle'
				@category_id = 4
			when 'player'
				@category_id = 5
			when 'game-type'
				@category_id = 7
			else
				@category_id = 1
		end

		# pull stats
		@commendations = [ ]
		H4Api.get_player_commendations(@gamertag)['Commendations'].each do |commendation|
			if (commendation['CategoryId'] == @category_id)
				@commendations << commendation
			end
		end
	end

end
