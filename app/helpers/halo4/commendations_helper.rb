module Halo4::CommendationsHelper
	def commendation_max_level(commendation_id)
		current_search_commendation_level = -69
		commendation_final = nil
		@metadata['CommendationsMetadata']['CommendationLevels'].each do |commendation|
			if commendation['CommendationId'] == commendation_id && current_search_commendation_level < commendation['Level']
				commendation_final = commendation
			end
		end
		return commendation_final['Level'] + 1
	end
end
