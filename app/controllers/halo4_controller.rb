class Halo4Controller < ApplicationController
	include ApplicationHelper
	include Halo4Helper
	require 'digest/md5'
	require 'active_support/core_ext/integer/inflections'

	def view_match
		@service_record = I343ApiH4.get_player_service_record(params[:gamertag])
		@match = I343ApiH4.get_match_details(params[:gamertag], params[:matchid])
		if @service_record['continue'] == 'no'
			# error, handle it
			setup_error_notification('error', I343ApiH4.error_message_from_status_code(@service_record['StatusCode'], params[:gamertag]))
			redirect_to_url :application
			return
		end
		if @match['continue'] == 'no'
			# error, handle it
			setup_error_notification('error', "The match `#{params[:matchid]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end

		@metadata = I343ApiH4.get_meta_data()
		@game_variant_metadata = @metadata['GameBaseVariantsMetadata']['GameBaseVariants']

		@match = @match['Game']
		@is_ffa = (@match['Teams'] != nil && @match['Teams'].count == 0)
		@top_player = @match['Players'].sort_by { |player| player['PersonalScore'] }.reverse[0]
		@sorted_players = @match['Players'].sort_by { |player| player['PersonalScore'] }.reverse

		if @top_player['PersonalScore'] == 0
			# sort by killz
			@sorted_players = @match['Players'].sort_by { |player| player['Kills'] }.reverse
			@top_player = @sorted_players[0]
		end

		# duration
		parts = @match['Duration'].split(':')
		@duration = Time.new(1994, 8, 18, parts[0].to_i, parts[1].to_i, parts[2].to_i, '+00:00')
	end
	def match_history
		@service_record = I343ApiH4.get_player_service_record(params[:gamertag])
		if @service_record['continue'] == 'no'
			# error, handle it
			setup_error_notification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end

		@recent_match_load_count = 36

		@start_index = 0
		@start_page = 0
		@selected_gamemode = 3
		if params[:game_mode].to_i != nil && params[:game_mode].to_i > 2 && params[:game_mode].to_i < 7
			@selected_gamemode = params[:game_mode].to_i
		end
		if params[:page].to_i != nil && params[:page].to_i > 0
			@start_index = params[:page].to_i * (@recent_match_load_count - 1)
			@start_page = params[:page].to_i
		end

		@metadata = I343ApiH4.get_meta_data()
		@commendations = I343ApiH4.get_player_commendations(params[:gamertag])['Commendations']
		@commendations = @commendations.sort { |a, b| [a['LevelId'], a['Ticks']] <=> [b['LevelId'], b['Ticks']] }.reverse
		@game_modes = @metadata['GameModesMetadata']['GameModes']
	end
	def competitive_skill_rank
		@service_record = I343ApiH4.get_player_service_record(params[:gamertag])
		if @service_record[:continue] == "no"
			# error, handle it
			setup_error_notification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end

		@metadata = I343ApiH4.get_meta_data()
	end
	def unique_playlist_csr
		@service_record = I343ApiH4.get_player_service_record(params[:gamertag])
		if @service_record[:continue] == "no"
			# error, handle it
			setup_error_notification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end

		@metadata = I343ApiH4.get_meta_data()
		@playlist_data = playlist_data_from_id(params[:playlist_id])

		if @playlist_data == nil
			# error, handle it
			setup_error_notification('error', "The specified playlist `#{params[:playlist_id]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end
	end
	# def summary
	# 	@service_record = I343ApiH4.get_player_service_record(params[:gamertag])
	# 	if @service_record['continue'] == 'no'
	# 		# error, handle it
	# 		setup_error_notification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
	# 		redirect_to '/'
	# 		return
	# 	end
	# 	@metadata = I343ApiH4.get_meta_data()

	# 	mode_data = I343ApiH4.GetDetailedModeDetails(params[:gamertag])
	# 	if mode_data['continue'] == 'no'
	# 		# error, handle it
	# 		setup_error_notification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
	# 		redirect_to '/'
	# 		return
	# 	end

	# 	@campaign_data = JSON.parse(mode_data.campaign_data)
	# 	@spartan_ops_data = JSON.parse(mode_data.spartan_ops_data)
	# 	@war_games_data = JSON.parse(mode_data.war_games_data)
	# 	@custom_data = JSON.parse(mode_data.custom_data)
	# end
	def specializations
		@service_record = I343ApiH4.get_player_service_record(params[:gamertag])
		if @service_record[:continue] == "no"
			# error, handle it
			setup_error_notification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end

		@metadata = I343ApiH4.get_meta_data()
	end
	def commendations
		@service_record = I343ApiH4.get_player_service_record(params[:gamertag])
		if @service_record[:continue] == "no"
			# error, handle it
			setup_error_notification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end

		@commendations = I343ApiH4.get_player_commendations(params[:gamertag])['Commendations'].sort_by { |commendation| commendation['NextLevel']['ProgressToNextLevel'] }.reverse
		@metadata = I343ApiH4.get_meta_data()
		@commendation_categories = @metadata['CommendationsMetadata']['CommendationCategories']
	end
	def service_record
		@service_record = I343ApiH4.get_player_service_record(params[:gamertag])
		if @service_record['continue'] == 'no'
			# error, handle it
			setup_error_notification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
			redirect_to '/'
			return
		end

		@metadata = I343ApiH4.get_meta_data()
		@recent_matches = I343ApiH4.get_player_matches(params[:gamertag], 0, 20)['Games']
	end

	def challenges
		@is_gamertag_related = false

		@challenge_metadata = I343ApiH4.get_meta_data['ChallengesMetadata']
		@challenge_categories = @challenge_metadata['ChallengeCategories']
		@challenge_periods = @challenge_metadata['ChallengePeriods']

		@friendly_challenge_data = { }
		challenges = I343ApiH4.get_challenge_data()['Challenges']
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
		@game_mode_metadata = I343ApiH4.get_meta_data()['GameModesMetadata']
		@game_modes = @game_mode_metadata['GameModes']
		@map_meta = I343ApiH4.get_meta_data()['GameModesMetadata']['Maps']
		@total_population = 0

		@friendly_playlist_data = { }
		playlists = I343ApiH4.get_playlist_data()['Playlists']
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





	# helpers
	def self.GetMedalDataFromId(medal_id, medal_meta)
		medal_meta.each do |medal|
			if medal['Id'] == medal_id
				return medal
			end
		end

		return nil
	end

	def self.GetMedalTeirFromId(tier_id, tier_meta)
		tier_meta.each do |tier|
			if tier['Id'] == tier_id
				return tier
			end
		end

		return nil
	end

	def self.GetVariantDataFromMeta(meta, variant_id)
		meta.each do |variant|
			if variant['Id'] == variant_id
				return variant
			end
		end

		return nil
	end
end