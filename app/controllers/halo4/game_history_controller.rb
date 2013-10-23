class Halo4::GameHistoryController < Halo4::HomeController
	include Halo4::GameHistoryHelper

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
		begin
			@game = H4Api.get_player_game(@service_record['Gamertag'], params[:game_id].to_s)['Game']
		rescue
			set_flash_message('failure', 'Oops..', "There was an error retreiving stats for this game.. The data was probally purged by 343. Sorry about that.")
			redirect_to(halo4_servicerecord_path(gamertag: @gamertag))
			return
		end

		@css_class = 'matchmaking'
		@partial_type = 'matchmaking'
		case @game['ModeId']
			when 3
				@css_class = 'matchmaking'
				@partial_type = 'matchmaking'
			when 4
				@css_class = 'campaign'
				@partial_type = 'campaign'
			when 5
				@css_class = 'spartan-ops'
				@partial_type = 'spartan-ops'
			when 6
				@css_class = 'custom-games'
				@partial_type = 'matchmaking'
		end


		@top_player = @game['Players'].sort_by { |p| p['Standing'] }[0]
		@teams_in_order = @game['Teams'].sort_by { |t| t['Standing'] } if (@game['ModeId'] == 3 || @game['ModeId'] == 6)
		@difficulty = get_full_difficulty(@game['Difficulty']) if (@game['ModeId'] == 4 || @game['ModeId'] == 5)
		@chapter = get_spops_chapter_from_chapter_id(@game['ChapterId']) if (@game['ModeId'] == 5)
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
