class Halo4::ServiceRecordController < Halo4::HomeController

	def index
		@game_history = H4Api.get_player_game_history(@gamertag, 0, 12)
	end

end
