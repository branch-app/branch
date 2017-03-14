module Halo4::ServiceRecordHelper
	def total_kills
		kills = 0
		@service_record['gameModes'].map do |mode|
			kills += mode['totalKills'].to_i
		end

		return kills
	end

	def total_deaths
		deaths = 0
		@service_record['gameModes'].map do |mode|
			deaths += mode['totalDeaths'].to_i
		end

		return deaths
	end

	def total_games_started
		games_started = 0
		@service_record['gameModes'].map do |mode|
			games_started += mode['totalGamesStarted'].to_i
		end

		return games_started
	end

	def generate_recent_match_json
		@recent_matches['games'].to_json
	end
end
