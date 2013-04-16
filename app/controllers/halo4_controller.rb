class Halo4Controller < ApplicationController
	require 'digest/md5'
	require 'active_support/core_ext/integer/inflections'


	def view_match
		@service_record = X343ApiController.GetServiceRecord(params[:gamertag])
		@match = X343ApiController.GetMatchDetails(params[:gamertag], params[:matchid])
		if @service_record['continue'] == 'no'
			# error, handle it
			SetupErrorNotification('error', X343ApiController.error_message_from_status_code(@service_record['StatusCode'], params[:gamertag]))
			redirect_to_url :application
			return
		end
		if @match['continue'] == 'no'
			# error, handle it
			SetupErrorNotification('error', "The match `#{params[:matchid]}` does not exist, or HaloWaypoint is down.")
			redirect_to_url :application
			return
		end

		@metadata = X343ApiController.GetMetaData()
		@game_variant_metadata = @metadata['GameBaseVariantsMetadata']['GameBaseVariants']

		@match = @match['Game']
		@is_ffa = @match['Teams'].count == 0
		@top_player = @match['Players'].sort_by { |player| player['PersonalScore'] }.reverse[0]
		@sorted_players = @match['Players'].sort_by { |player| player['PersonalScore'] }.reverse

		# duration
		parts = @match['Duration'].split(':')
		@duration = Time.new(1994, 8, 18, parts[0].to_i, parts[1].to_i, parts[2].to_i, '+00:00')
	end

	def match_history
		@service_record = X343ApiController.GetServiceRecord(params[:gamertag])
		if @service_record['continue'] == 'no'
			# error, handle it
			SetupErrorNotification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
			redirect_to_url :application
			return
		end

		@start_index = 0
		@selected_gamemode = 3
		if params[:game_mode].to_i != nil && params[:game_mode].to_i > 2 && params[:game_mode].to_i < 7
			@selected_gamemode = params[:game_mode].to_i
		end
		if params[:page].to_i != nil && params[:page].to_i > 0
			@start_index = params[:page].to_i * 25
		end

		@metadata = X343ApiController.GetMetaData()
		@commendations = X343ApiController.GetPlayerCommendations(params[:gamertag])['Commendations']
		@commendations = @commendations.sort { |a, b| [a['LevelId'], a['Ticks']] <=> [b['LevelId'], b['Ticks']] }.reverse
		@game_modes = @metadata['GameModesMetadata']['GameModes']
	end

	def service_record
		@service_record = X343ApiController.GetServiceRecord(params[:gamertag])
		if @service_record['continue'] == 'no'
			# error, handle it
			SetupErrorNotification('error', "The gamertag `#{params[:gamertag]}` does not exist, or HaloWaypoint is down.")
			redirect_to_url :application
			return
		end

		@recent_matches = X343ApiController.GetPlayerMatches(params[:gamertag], 0, 20)['Games']
		@commendations = X343ApiController.GetPlayerCommendations(params[:gamertag])['Commendations']
		@commendations = @commendations.sort { |a, b| [a['LevelId'], a['Ticks']] <=> [b['LevelId'], b['Ticks']] }.reverse
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
		challenges = JSON.parse(H4GlobalChallenges.first.data)['Challenges']
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
		playlists = JSON.parse(H4Playlists.first.data)['Playlists']
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
	def self.CampaignProgressFromId(progress1, progress2, size)
		use_progress_1 = true if progress1 > progress2

		if progress1 == nil || progress2 == 0
			progress1 = 'easy'
		end
		if progress1 == 1
			progress1 = 'normal'
		end
		if progress1 == 2
			progress1 = 'heroic'
		end
		if progress1 == 3
			progress1 = 'legendary'
		end

		if progress2 == nil || progress2 == 0
			progress2 = 'easy'
		end
		if progress2 == 1
			progress2 = 'normal'
		end
		if progress2 == 2
			progress2 = 'heroic'
		end
		if progress2 == 3
			progress2 = 'legendary'
		end


		if use_progress_1
			return X343ApiController.asset_url_generator_basic('H4DifficultyAssets', "{size}/#{progress1}.png", size)
		else
			return X343ApiController.asset_url_generator_basic('H4DifficultyAssets', "{size}/#{progress2}.png", size)
		end
	end

	def self.GetHighestSkillRank(skill_ranks)
		highest_skill = 0

		skill_ranks.each do |rank|
			if rank['CurrentSkillRank'] == nil
				rank['CurrentSkillRank'] = 0
			end

			if rank['CurrentSkillRank'] > highest_skill
				highest_skill = rank['CurrentSkillRank']
			end
		end

		return highest_skill
	end

	def self.GetSimpleRankData(service_record, size)
		h4_rank_data = { }

		# current rank data
		h4_rank_data[:current_rank_name] = service_record['RankName']
		h4_rank_data[:current_rank_id] = service_record['RankId']
		h4_rank_data[:current_rank_url] = X343ApiController.asset_url_generator_basic(service_record['RankImageUrl']['BaseUrl'], service_record['RankImageUrl']['AssetUrl'], size)
		h4_rank_data[:current_rank_start_xp] = service_record['RankStartXP']

		# next rank data
		h4_rank_data[:next_rank_name] = service_record['NextRankName']
		h4_rank_data[:next_rank_id] = service_record['NextRankId']
		h4_rank_data[:next_rank_url] = X343ApiController.asset_url_generator_basic(service_record['NextRankImageUrl']['BaseUrl'], service_record['NextRankImageUrl']['AssetUrl'], size)
		h4_rank_data[:next_rank_start_xp] = service_record['NextRankStartXP']

		# specialization stuff
		h4_rank_data[:specialization_block] = nil
		service_record['Specializations'].each do |specialization|
			if specialization['IsCurrent']
				h4_rank_data[:specialization_block] = specialization
			end
		end
		if h4_rank_data[:specialization_block]['Completed']
			h4_rank_data[:specialization_block]['Name'] == 'Mastery'
		end

		# percentage stuff
		if service_record['RankStartXP'] == 0
			h4_rank_data[:update_next_rank_name] = 'Max Rank'
			h4_rank_data[:update_upper_xp] = ''
			h4_rank_data[:update_current_xp] = service_record['XP']
			h4_rank_data[:update_percentage] = 100
		else
			h4_rank_data[:update_next_rank_name] = service_record['NextRankName']
			h4_rank_data[:update_upper_xp] = service_record['NextRankStartXP'] - service_record['RankStartXP']
			h4_rank_data[:update_current_xp] = service_record['XP'] - service_record['RankStartXP']

			a = h4_rank_data[:update_current_xp].to_i
			b = h4_rank_data[:update_upper_xp].to_i

			perc = ((a.to_f / b.to_f) * 100)

			h4_rank_data[:update_percentage] = perc
		end

		return h4_rank_data
	end

	def self.RecentGameStyleFromResult(entry)
		if !entry['Completed']
			return 'recent-match-dnf'
		elsif entry['Result'] == 0
			return 'recent-match-los'
		else
			return 'recent-match-win'
		end
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
