module Halo4::ServiceRecordHelper
	def rank_progression_percentage
		xp = @service_record['xp']
		rank_start_xp = @service_record['rankStartXp']
		next_rank_start_xp = @service_record['nextRankStartXp']

		percentage = generate_percentage(xp - rank_start_xp, next_rank_start_xp - rank_start_xp)
		percentage = 100 if @service_record['nextRankName'] == ''

		return percentage
	end

	def rank_progression_comparison
		xp = @service_record['xp']
		rank_start_xp = @service_record['rankStartXp']
		next_rank_start_xp = @service_record['nextRankStartXp']

		return ' - Max Rank Achieved' if @service_record['nextRankName'] == ''

		return " (#{xp}/#{next_rank_start_xp})"
	end

	def played_games_spread_json
		played_games = Array.new(@service_record['gameModes'].length)
		@service_record['gameModes'].each_with_index do |mode, index|
			played_games[index] = mode['totalGamesStarted']
		end
		return played_games.to_json
	end

	def played_games_spread_legend_json
		played_games_legend = Array.new(@service_record['gameModes'].length)
		@service_record['gameModes'].each_with_index do |mode, index|
			played_games_legend[index] = mode['name']
		end
		return played_games_legend.to_json
	end
end
