class ApplicationController < ActionController::Base
  protect_from_forgery

	def index
		# Challanges
		@challenge_metadata = X343ApiController.GetMetaData()['ChallengesMetadata']
		@challenge_categories = @challenge_metadata['ChallengeCategories']
		@challenge_periods = @challenge_metadata['ChallengePeriods']

		@friendly_challenge_data = { }
		challenges = X343ApiController.GetChallengeData()['Challenges']
		@challenge_categories.each do |category|
			relevant_challenges = [ ]
			challenges.each do |challenge|
				if challenge['CategoryId'] == category['Id']
					relevant_challenges << challenge
				end
			end
			@friendly_challenge_data[category['Id']] = relevant_challenges
		end


		# H4Playlists
		@game_mode_metadata = X343ApiController.GetMetaData()['GameModesMetadata']
		@game_modes = @game_mode_metadata['GameModes']
		@total_population = 0

		@friendly_playlist_data = { }
		playlists = X343ApiController.GetPlaylistData()['Playlists']
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

		# Blogs
		@blog_posts = BlogPost.where('is_published=1').order('id ASC')
	end

	def about

	end
end