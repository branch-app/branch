module Halo4::HomeHelper
	#-- Service Record --#
	def get_highest_csr(top_skill_rank)
		if (top_skill_rank != nil)
			return top_skill_rank['CurrentSkillRank']
		end
		return 0
	end

	#-- Asset Urls --#
	def get_player_model_url(gamertag, size = 'large', pose = 'fullbody')
		player_model_url = H4Api.get_player_model(gamertag, size, pose)

		if (GamertagReplacement.find_by_target(gamertag) != nil)
			return "data:image/png;base64,#{H4Api.get_supersecret_player_model(player_model_url)}"
		else
			return player_model_url
		end
	end

	def get_raw_asset_url(raw_asset_object, size = 'medium')
		return H4Api.asset_url_generator_basic(raw_asset_object['BaseUrl'], raw_asset_object['AssetUrl'], size.to_s)
	end

	def get_csr_level_url(csr, size = 'medium', version = 'v1')
		return "https://assets.halowaypoint.com/games/h4/csr/#{version}/#{size}/#{csr}.png"
	end

	#-- Get Shit From an Id --#
	def get_map_from_base_id(base_id)
		@metadata['MapsMetadata']['Maps'].each do |map|
			if (map['Id'] == base_id.to_i)
				return map
			end
		end

		return nil
	end

	def get_gametype_from_base_id(base_id)
		@metadata['GameBaseVariantsMetadata']['GameBaseVariants'].each do |game_variant|
			if (game_variant['Id'] == base_id)
				return game_variant
			end
		end

		return nil
	end

	def get_spops_chapter_from_chapter_id(chapter_id)
		@metadata['SpartanOpsMetadata']['Seasons'].each do |season|
			season['Episodes'].each do |episode|
				episode['Chapters'].each do |chapter|
					if chapter['Id'] == chapter_id
						return chapter
					end
				end
			end
		end

		return nil
	end
end
