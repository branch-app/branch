class Halo4::GameHistoryController < Halo4::HomeController

	def index
		@sub_view, sub_view_was_valid = validate_param(params[:sub_view], [ 'matchmaking', 'custom-games', 'spartan-ops', 'campaign' ], 'matchmaking')
		@page, page_was_valid = validate_numerical_param(params[:page])

		if (!page_was_valid || !sub_view_was_valid)
			redirect_to(halo4_gamehistory_path( sub_view: @sub_view || 'matchmaking', page: @page || 0 ))
			return
		end

		@is_last_page = true
		@friendly_name = sub_view_to_friendly(@sub_view)
		@css_class = sub_view_to_css_class(@sub_view)


		@lower_bounds = @page * 25
		@mode_id = mode_id_from_mode_name(@sub_view)
		@game_history = H4Api.get_player_game_history(@gamertag, @lower_bounds, 26, @mode_id)

		if (@game_history['Games'].length == 26)
			@is_last_page = false
			@game_history['Games'].pop(1)
		end
	end

	def view
		@css_class = 'matchmaking'
		@duration = Time.new(1994, 8, 18, 2, 45, 56, '+00:00')
	end
	
	private
		def mode_id_from_mode_name(mode_name)
			case mode_name
				when 'matchmaking'
					return 3
				when 'campaign'
					return 4
				when 'spartan-ops'
					return 5
				when 'custom-games'
					return 6
				else
					return 3
			end
		end
end
