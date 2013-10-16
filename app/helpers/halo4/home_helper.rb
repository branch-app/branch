module Halo4::HomeHelper
	#-- Service Record --#
	def get_highest_csr(top_skill_rank)
		if (top_skill_rank != nil)
			return top_skill_rank['CurrentSkillRank']
		end
		return 0
	end

	#-- Asset Urls --#
	def get_player_model_url(gamertag, size, pose = 'fullbody')
		return H4Api.get_player_model(gamertag, size, pose)
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
end
