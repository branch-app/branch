class Halo4::ChallengesController < Halo4::HomeController

	def index
		@challenge_data = H4Api.get_challenge_data()['Challenges']
		@challenge_categories = [ ]

		@challenge_data.each do |challenge|
			# check we no have category
			we_have = false
			@challenge_categories.map{ |c| we_have = true if (c['Id'] == challenge['CategoryId']) }

			if (!we_have)
				@challenge_categories << get_challenge_category_from_base_id(challenge['CategoryId'])
			end
		end
	end
end
