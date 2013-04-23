module Halo4Helper
	require 'active_support/core_ext/integer/inflections'

	# playlist
	def playlist_data_from_id(playlist_id)
		@metadata['PlaylistsMetadata']['Playlists'].each do |playlist|
			if playlist['Id'] == playlist_id.to_i
				return playlist
			end
		end

		return
	end

	def draw_game_count_chart(service_record)
		output = ''

		index = 0
		service_record['GameModes'].each do |game_mode|
			index += 1
			output += "{ label: '#{game_mode['Name']}', value: #{game_mode['TotalGamesStarted']}, index: '#{index}'},"
		end
		return output
	end

	def create_overview_from_service_record(service_record)
		overview_data = {
			:last_played => { :title => 'Last Played', :body => DateTime.strptime(service_record['LastPlayedUtc'], '%Y-%m-%dT%H:%M:%SZ').strftime('%d.%m.%Y') },
			:loaduout_completion => { :title => 'Loadout Purchase Completion', :body => service_record['TotalLoadoutItemsPurchased'].to_s + '%' },
			:challenge_completion => { :title => 'Challenges Completed', :body => service_record['TotalChallengesCompleted'] },
			:spartan_points => { :title => 'Spartan Points', :body => number_with_delimiter(service_record['SpartanPoints'], :delimiter => ',') },
			:total_kills => { :title => 'War Games Kills', :body => "#{number_with_delimiter(service_record['GameModes'][2]['TotalKills'], :delimiter => ',')} (#{service_record['GameModes'][2]['KDRatio']})" },
			:total_medals => { :title => 'War Games Medals', :body => "#{number_with_delimiter(service_record['GameModes'][2]['TotalMedals'], :delimiter => ',')}" },
			:covenant_killed => { :title => 'Covenant/Promethean Kills', :body => "#{number_with_delimiter(service_record['GameModes'][0]['TotalKills'] + service_record['GameModes'][1]['TotalKills'], :delimiter => ',')}" },
			:games_started => { :title => 'Games Started', :body => "#{number_with_delimiter(service_record['TotalGamesStarted'], :delimiter => ',')}" },
			:player_since => { :title => 'Player Since', :body => DateTime.strptime(service_record['FirstPlayedUtc'], '%Y-%m-%dT%H:%M:%SZ').strftime('%d.%m.%Y') }
		}

		return overview_data
	end

	def update_overview_type_index(type_index)
		if type_index == 0
			type_index += 1
		else
			type_index = 0
		end

		return type_index
	end

	def get_simple_rank_data(service_record, size)
		h4_rank_data = { }

		# current rank data
		h4_rank_data[:current_rank_name] = service_record['RankName']
		h4_rank_data[:current_rank_id] = service_record['RankId']
		h4_rank_data[:current_rank_url] = X343ApiController.asset_url_generator_basic(service_record['RankImageUrl']['BaseUrl'], service_record['RankImageUrl']['AssetUrl'], size)
		h4_rank_data[:current_rank_start_xp] = service_record['RankStartXP']

		# next rank data
		if service_record['NextRankId'] == 0
			# max rank
			h4_rank_data[:next_rank_name] = 'Mastery'
			h4_rank_data[:next_rank_name_friendly] = 'Mastery (Max Rank)'
			h4_rank_data[:next_rank_id] = service_record['RankId']
			h4_rank_data[:next_rank_url] = X343ApiController.asset_url_generator_basic(service_record['RankImageUrl']['BaseUrl'], service_record['RankImageUrl']['AssetUrl'], size)
			h4_rank_data[:next_rank_start_xp] = service_record['NextRankStartXP']
		else
			h4_rank_data[:next_rank_name] = service_record['NextRankName']
			h4_rank_data[:next_rank_name_friendly] = "#{service_record['NextRankName']} (#{service_record['NextRankStartXP']})"
			h4_rank_data[:next_rank_id] = service_record['NextRankId']
			h4_rank_data[:next_rank_url] = X343ApiController.asset_url_generator_basic(service_record['NextRankImageUrl']['BaseUrl'], service_record['NextRankImageUrl']['AssetUrl'], size)
			h4_rank_data[:next_rank_start_xp] = service_record['NextRankStartXP']
		end

		# specialization stuff
		h4_rank_data[:specialization_block] = nil
		service_record['Specializations'].each do |specialization|
			if specialization['IsCurrent']
				h4_rank_data[:specialization_block] = specialization
			end
		end
		if h4_rank_data[:specialization_block]['PercentComplete'] == 1.0
			h4_rank_data[:specialization_block]['Name'] = 'Mastery'
		end

		# percentage stuff
		if service_record['NextRankId'] == 0
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

	def campaign_progress_from_id(progress1, progress2, size)
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

	def difficulty_from_id(difficulty_id)
		return @metadata['DifficultiesMetadata']['Difficulties'][difficulty_id]
	end

	def recent_game_style_from_result(entry)
		if entry['ModeName'] == 'Spartan Ops' || entry['ModeName'] == 'Campaign'
			unless entry['Completed']
				return 'recent-match-dnf'
			else
				return 'recent-match-win'
			end
		end

		if !entry['Completed']
			return 'recent-match-dnf'
		elsif entry['Result'] == 0
			return 'recent-match-los'
		else
			return 'recent-match-win'
		end
	end

	def get_spartan_ops_chapter(season_id, episode_id, chapter_id, meta)
		meta['SpartanOpsMetadata']['Seasons'].each do |season|
			if season['Id'] == season_id
				season['Episodes'].each do |episode|
					if episode['Id'] == episode_id
						episode['Chapters'].each do |chapter|
							if chapter['Id'] == chapter_id
								return chapter
							end
						end
					end
				end
			end
		end
		return nil
	end
	def get_spartan_ops_data_from_chapter_id(chapter_id)
		@metadata['SpartanOpsMetadata']['Seasons'].each do |season|
			season['Episodes'].each do |episode|
				episode['Chapters'].each do |chapter|
					if chapter['Id'] == chapter_id
						return chapter
					end
				end
			end
		end
	end

	def calculate_ratio(kills, deaths, round_to = 2)
		if kills <= 0 || deaths <= 0
			return 1
		else
			(kills.to_f / deaths.to_f).to_f.round(round_to)
		end
	end

	def skull_meta_from_id(skull_id)
		@metadata['SkullsMetadata']['Skulls'].each do |skull|
			if skull['Id'] == skull_id
				return skull
			end
		end

		return nil
	end

	def draw_war_games_carnage_graph(type)
		output = ''
		if @is_ffa
		  @sorted_players.each do |player|
			  tick_count = 0
			  player[type].sort_by { |thingy| thingy['Time'] }.each do |thingy|
				  tick_count += thingy['Ticks']
				  output += "{ t: #{thingy['Time']}, player#{player['Gamertag'].gsub(/[^0-9A-Za-z]/, '')}: #{tick_count} }, "
			  end
		  end
		else
			@match['Teams'].each do |team|
				tick_count = 0
				team[type].sort_by { |thingy| thingy['Time'] }.each do |thingy|
					tick_count += thingy['Ticks']
					output += "{ t: #{thingy['Time']}, team#{team['Id']}: #{tick_count} }, "
				end
			end
		end

		return output
	end
	def create_war_games_carnage_graph_ykeys
		ykeys = ''
		if @is_ffa
		  @sorted_players.each do |player|
			  ykeys += '\'player' + player['Gamertag'].gsub(/[^0-9A-Za-z]/, '') + '\', '
			end
		  ykeys = ykeys[0...-2]
		else
	    @match['Teams'].each do |team|
	      ykeys += '\'team' + team['Id'].to_s + '\', '
	    end
	    ykeys = ykeys[0...-2]
		end

		return ykeys
	end
	def create_war_games_carnage_graph_labels
		labels = ''
		if @is_ffa
			@sorted_players.each do |player|
				labels += '\'' + player['Gamertag'] + '\', '
			end
			return labels
		end

		return ''
	end
	def create_war_games_carnage_graph_line_colours
		# hahaha, english spelling, fuck you.
		if @is_ffa
			return ''
		end

		output = 'lineColors: [ '
		@match['Teams'].each do |team|
			output += "'#{team['PrimaryRGB']}', "
		end
		return output[0...-2] += ' ]'
	end

	def draw_spartan_ops_carnage_graph(type)
		output = ''
		@sorted_players.each do |player|
			tick_count = 0
			player[type].sort_by { |thingy| thingy['Time'] }.each do |thingy|
				tick_count += thingy['Ticks']
				output += "{ t: #{thingy['Time']}, player#{player['Gamertag'].gsub(/[^0-9A-Za-z]/, '')}: #{tick_count} }, "
			end
		end

		return output
	end
	def create_spartan_ops_carnage_graph_ykeys
		ykeys = ''
		@sorted_players.each do |player|
			ykeys += '\'player' + player['Gamertag'].gsub(/[^0-9A-Za-z]/, '') + '\', '
		end
		ykeys = ykeys[0...-2]

		return ykeys
	end
	def create_spartan_ops_carnage_graph_labels
		labels = ''
		@sorted_players.each do |player|
			labels += '\'' + player['Gamertag'] + '\', '
		end
		return labels
	end
	def create_spartan_ops_carnage_graph_line_colours
		# hahaha, english spelling, fuck you.
			return ''
	end

	def place_from_standing(standing)
		if standing == -1
			return 'DNF'
		else
			standing.ordinalize
		end
	end

	def match_history_pagnation_next(mode_id)
		if mode_id == @selected_gamemode
			# yolo continue on like a nigga
			return "/halo4/servicerecord/#{@service_record['Gamertag']}/match-history/#{mode_id}/#{@start_page + 1}/"
		else
			# woah, this one was unpopular, lets drop out of boarding school and do something with your life
			return "/halo4/servicerecord/#{@service_record['Gamertag']}/match-history/#{mode_id}/2/"
		end
	end
	def match_history_pagnation_previous(mode_id)
		if mode_id == @selected_gamemode
			# yolo continue on like a nigga
			return "/halo4/servicerecord/#{@service_record['Gamertag']}/match-history/#{mode_id}/#{@start_page - 1}/"
		end
	end

	# specializations
	def get_in_progress_specialization
		@service_record['Specializations'].each do |specialization|
			if specialization['IsCurrent']
				if specialization['PercentComplete'] == 1.0
					return nil
				else
					return specialization
				end
			end
		end
		return nil
	end
	def get_pending_specializations
		pending_specializations = [ ]
		@service_record['Specializations'].each do |specialization|
			if !specialization['IsCurrent'] && !specialization['Completed']
				pending_specializations << specialization
			end
		end
		return pending_specializations
	end
	def get_completed_specializations
		completed_specializations = [ ]
		@service_record['Specializations'].each do |specialization|
			if !specialization['IsCurrent'] && specialization['Completed']
				completed_specializations << specialization
			end
		end
		return completed_specializations
	end
	def specialization_data_from_id(specialization_id)
		@metadata['SpecializationsMetadata']['Specializations'].each do |specialization|
			if specialization['Id'] == specialization_id
				return specialization
			end
		end
		return nil
	end

	# commendations
	def plural_commendation_category_name_to_single(name)
		case name
			when 'Weapons'
				return 'Weapon'
			when 'Enemies'
				return 'Enemy'
			when 'Vehicles'
				return 'Vehicle'
			when 'Player'
				return 'Player'
			when 'Game Types'
				return 'Game Type'
			else
				return ''
		end
	end
	def commendations_by_category(category_id)
		commendations = [ ]
		@commendations.each do |commendation|
			if commendation['CategoryId'].to_i == category_id.to_i
				commendations << commendation
			end
		end
		return commendations
	end
	def commendation_max_level(commendation_id)
		current_search_commendation_level = -69
		commendation_final = nil
		@metadata['CommendationsMetadata']['CommendationLevels'].each do |commendation|
			if commendation['CommendationId'] == commendation_id && current_search_commendation_level < commendation['Level']
				commendation_final = commendation
			end
		end
		return commendation_final
	end

	# competitive skill rank
	def csr_graph_labels(type)
		playlist_ids = [ ]
		if type == 'team'
			playlist_ids = [129,122,105,102,123,121,117]
		else
			playlist_ids = [100,101,128,116,126,104,113,103,115]
		end
		output = ''

		@service_record['SkillRanks'].each do |skill_ranks|
			if playlist_ids.include? skill_ranks['PlaylistId']
				output += "'#{skill_ranks['PlaylistName']}', "
			end
		end

		return output[0...-2]
	end
	def csr_graph_datapoints(type)
		playlist_ids = [ ]
		if type == 'team'
			playlist_ids = [129,122,105,102,123,121,117]
		else
			playlist_ids = [100,101,128,116,126,104,113,103,115]
		end
		output = ''
		@service_record['SkillRanks'].each do |skill_ranks|
			tmp_output = ''
			if skill_ranks['CurrentSkillRank'] == nil
				tmp_output += '0, '
			else
				tmp_output += skill_ranks['CurrentSkillRank'].to_s + ', '
			end

			if playlist_ids.include? skill_ranks['PlaylistId']
				output += tmp_output
			end
		end
		return output[0...-2]
	end
	def csr_data_from_type(type)
		playlist_ids = [ ]
		if type == 'team'
			playlist_ids = [129,122,105,102,123,121,117]
		else
			playlist_ids = [100,101,128,116,126,104,113,103,115]
		end
		skill_ranks = [ ]

		@service_record['SkillRanks'].each do |skill_rank|
			if playlist_ids.include? skill_rank['PlaylistId']
				skill_ranks << skill_rank
			end
		end

		return skill_ranks
	end
	def get_highest_skill_rank(skill_ranks)
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
	def csr_image_from_raw_csr(raw_csr, size)
		if raw_csr == nil
			return "https://assets.halowaypoint.com/games/h4/csr/v1/#{size}/0.png"
		else
			return "https://assets.halowaypoint.com/games/h4/csr/v1/#{size}/#{raw_csr['GameSkillRank']}.png"
		end
	end
	def csr_image_from_current_csr(raw_csr, size)
		if raw_csr == nil
			return "https://assets.halowaypoint.com/games/h4/csr/v1/#{size}/0.png"
		else
			return "https://assets.halowaypoint.com/games/h4/csr/v1/#{size}/#{raw_csr}.png"
		end
	end
	def skill_rank_from_playlist_id(playlist_id)
		@service_record['SkillRanks'].each do |skill_rank|
			if skill_rank['PlaylistId'] == playlist_id.to_i
				return skill_rank
			end
		end

		return nil
	end
end
