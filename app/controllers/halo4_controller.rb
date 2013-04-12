class Halo4Controller < ApplicationController

	def service_record
		render :text => params[:gamertag]
	end

	def challenges
		@is_gamertag_related = false

		@challenge_metadata = X343ApiController.GetMetaData['ChallengesMetadata']
		@challenge_categories = @challenge_metadata['ChallengeCategories']
		@challenge_periods = @challenge_metadata['ChallengePeriods']

		if params[:gamertag] != nil
			# gamurtag
			@friendly_challenge_data = { }
			challenges = X343ApiController.GetPlayerChallenges(params[:gamertag])

			if challenges != 'player_not_found'
				flash[:notice] = "Specified Player `#{params[:gamertag]}` could not be found."
				flash[:type] = 'error'
			else
				challenges = challenges['Challenges']
				@challenge_categories.each do |category|
					relevant_challenges = [ ]
					challenges.each do |challenge|
						if challenge['CategoryId'] == category['Id']
							relevant_challenges << challenge
						end
					end
					@friendly_challenge_data[category['Id']] = relevant_challenges
					@is_gamertag_related = true
					return
				end
			end
		end

		@friendly_challenge_data = { }
		challenges = JSON.parse(GlobalChallenges.first.data)['Challenges']
		@challenge_categories.each do |category|

			relevant_challenges = [ ]
			challenges.each do |challenge|
				if challenge['CategoryId'] == category['Id']
					relevant_challenges << challenge
				end
			end
			@friendly_challenge_data[category['Id']] = relevant_challenges
		end
	end

	def playlists
		@game_mode_metadata = X343ApiController.GetMetaData()['GameModesMetadata']
		@game_modes = @game_mode_metadata['GameModes']
		@map_meta = X343ApiController.GetMetaData()['GameModesMetadata']['Maps']
		@total_population = 0

		@friendly_playlist_data = { }
		playlists = JSON.parse(Playlists.first.data)['Playlists']
		@game_modes.each do |mode|
			relevant_playlists = [ ]
			playlists.each do |playlist|
				if playlist['ModeId'] == mode['Id']
					relevant_playlists << playlist

					@total_population += playlist['PopulationCount']
				end
			end
			@friendly_playlist_data[mode['Id']] = relevant_playlists
		end
	end
end
